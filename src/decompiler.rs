use std::{collections::HashMap, path::Path};

use crate::token::{
    lexer, FunctionCall, Operation, ScoreboardObjectivesAdd, ScoreboardPlayersOperation,
    ScoreboardPlayersSet, Token,
};

#[derive(Debug)]
pub struct DecompilerSettings {
    pub indent: usize,
}

#[derive(Debug)]
pub struct DecompilerContext {
    pub settings: DecompilerSettings,
    pub name: Option<String>,
    pub functions: Vec<Function>,
    pub entry_function: Option<Function>,
}

#[derive(Debug)]
pub struct DecompilerResult {
    pub name: String,
    pub functions: Vec<DecompiledFunction>,
    pub entry_function: DecompiledFunction,
}

#[derive(Debug)]
pub struct DecompiledFunction {
    pub name: String,
    pub text: String,
}

#[derive(Debug)]
pub struct Function {
    pub name: String,
    pub args: Option<HashMap<usize, String>>,
    pub body: Option<Vec<Instruction>>,
}

// define Statement enum
#[derive(Debug)]
pub enum Instruction {
    PlayersSet(PlayersSet),
    PlayersOperation(PlayersOperation),
    FunctionCall(Call),
    Comment(String),
}

#[derive(Debug)]
pub struct PlayersSet {
    tmp: String,
    val: i32,
}

#[derive(Debug)]
pub struct PlayersOperation {
    tmp: String,
    arg: String,
    op: Operation,
}

#[derive(Debug)]
pub struct Call {
    namespace: String,
    func_name: String,
}

// TODO: I got lazy and started using panic instead of Results/Options...

pub fn decompile(dir: &Path, settings: DecompilerSettings) -> Result<DecompilerResult, String> {
    let mut context = DecompilerContext {
        settings,
        name: None,
        functions: Vec::new(),
        entry_function: None,
    };
    let mut result = None;

    match collect_functions(dir, context) {
        Ok(ctx) => context = ctx,
        Err(e) => return Err(e.to_string()),
    }

    match write_functions(context) {
        Ok(res) => result = Some(res),
        Err(e) => return Err(e.to_string()),
    }

    match result {
        Some(res) => Ok(res),
        None => Err(String::from("uh oh")),
    }
}

fn collect_functions(
    dir: &Path,
    mut context: DecompilerContext,
) -> Result<DecompilerContext, std::io::Error> {
    for entry in std::fs::read_dir(dir)? {
        let entry = entry?;
        let path = entry.path();

        if path.is_dir() || path.extension().unwrap() != "mcfunction" {
            continue;
        }

        let file = std::fs::read_to_string(path)?;
        let tokens = lexer(&file).collect::<Vec<_>>();

        // get file name without dir or extension
        let name = &entry
            .path()
            .file_stem()
            .unwrap()
            .to_str()
            .unwrap()
            .to_string()
            .to_lowercase();

        match name.as_str() {
            "_sculkmain" => {
                let mut func = Function {
                    name: String::from(name),
                    args: None,
                    body: None,
                };
                let project_name;
                (project_name, func) = build_entry_body(tokens, func);
                context.name = Some(project_name.unwrap());
                context.entry_function = Some(func);
            }
            _ => {
                let func = Function {
                    name: String::from(name),
                    args: None,
                    body: None,
                };
                context.functions.push(build_function(tokens, func));
            }
        }
    }

    Ok(context)
}

fn build_entry_body(tokens: Vec<Token>, func: Function) -> (Option<String>, Function) {
    let name;

    if tokens.len() != 2 {
        panic!("Invalid entry function");
    }

    match &tokens[0] {
        Token::ScoreboardObjectivesAdd(soa) => {
            if soa.objective != "_SCULK" {
                panic!("Invalid entry function");
            }
        }
        _ => panic!("Invalid entry function"),
    }

    match &tokens[1] {
        Token::FunctionCall(fc) => {
            /*if fc.func_name != "main" {
                panic!("Invalid entry function");
            }*/
            name = Some(fc.namespace.clone());
        }
        _ => panic!("Invalid entry function"),
    }

    (name, build_function(tokens, func))
}

fn build_function(tokens: Vec<Token>, mut func: Function) -> Function {
    println!("{:#?}", tokens);

    func.args = Some(HashMap::new());
    let mut instructions = Vec::new();

    for token in tokens {
        match token {
            Token::ScoreboardObjectivesAdd(soa) => {
                // Doesn't really translate yet...
                instructions.push(Instruction::Comment(format!(
                    "objects add {}",
                    soa.objective
                )));
            }
            Token::ScoreboardPlayersSet(sps) => {
                if sps.objective != "_SCULK" {
                    panic!("Unexpected objective");
                }

                instructions.push(Instruction::PlayersSet(PlayersSet {
                    tmp: sps.target.clone(),
                    val: sps.score,
                }));
            }
            Token::ScoreboardPlayersOperation(spo) => {
                if spo.source_objective != "_SCULK" || spo.target_objective != "_SCULK" {
                    panic!("Unexpected objective(s)");
                }

                instructions.push(Instruction::PlayersOperation(PlayersOperation {
                    tmp: spo.target.clone(),
                    arg: spo.source.clone(),
                    op: spo.operation.clone(),
                }));
            }
            Token::FunctionCall(fc) => {
                instructions.push(Instruction::FunctionCall(Call {
                    namespace: fc.namespace.clone(),
                    func_name: fc.func_name.clone(),
                }));
            }
            _ => {}
        }
    }

    print!("{:#?}", instructions);

    func.body = Some(instructions);
    func
}

fn write_functions(mut context: DecompilerContext) -> Result<DecompilerResult, std::io::Error> {
    let mut entry_function = None;
    let mut decompiled_functions = Vec::new();

    if let Some(func) = context.entry_function {
        entry_function = Some(decompile_function(func, &context.settings));
    }

    for func in context.functions {
        let decompiled_function = decompile_function(func, &context.settings);
        decompiled_functions.push(decompiled_function);
    }

    Ok(DecompilerResult {
        name: context.name.unwrap(),
        functions: decompiled_functions,
        entry_function: entry_function.unwrap(),
    })
}

fn decompile_function(func: Function, settings: &DecompilerSettings) -> DecompiledFunction {
    let mut decompiled_function = DecompiledFunction {
        name: func.name.clone(),
        text: String::new(),
    };

    let mut lines = Vec::new();

    for instruction in func.body.unwrap() {
        match instruction {
            Instruction::PlayersSet(ps) => {
                lines.push(format!("{} = {}", ps.tmp, ps.val));
            }
            Instruction::PlayersOperation(po) => {
                lines.push(format!(
                    "{} = {} {} {}",
                    po.tmp,
                    po.tmp,
                    po.op.to_string(),
                    po.arg
                ));
            }
            Instruction::FunctionCall(fc) => {
                lines.push(format!("{}({})", fc.func_name, fc.namespace));
            }
            Instruction::Comment(c) => {
                lines.push(format!("// {}", c));
            }
        }
    }

    decompiled_function.text = lines.join("\n");

    decompiled_function
}
