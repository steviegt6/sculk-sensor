use std::path::Path;

use crate::token::{
    lexer, FunctionCall, ScoreboardObjectivesAdd, ScoreboardPlayersOperation, ScoreboardPlayersSet,
    Token,
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
pub struct Function {
    pub name: String,
    pub body: Option<Vec<Statement>>,
}

// define Statement enum
#[derive(Debug)]
pub enum Statement {
    ScoreboardObjectivesAdd(ScoreboardObjectivesAdd),
    ScoreboardPlayersSet(ScoreboardPlayersSet),
    ScoreboardPlayersOperation(ScoreboardPlayersOperation),
    FunctionCall(FunctionCall),
}

// TODO: I got lazy and started using panic instead of Results/Options...

pub fn decompile(dir: &Path, settings: DecompilerSettings) -> Result<DecompilerContext, String> {
    let mut context = DecompilerContext {
        settings,
        name: None,
        functions: Vec::new(),
        entry_function: None,
    };

    match collect_functions(dir, context) {
        Ok(ctx) => context = ctx,
        Err(e) => return Err(e.to_string()),
    }

    Ok(context)
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

fn build_function(tokens: Vec<Token>, func: Function) -> Function {
    println!("{:#?}", tokens);

    let mut statements = Vec::new();
    let mut i = 0;

    while i < tokens.len() {
        match &tokens[i] {
            Token::ScoreboardObjectivesAdd(soa) => {
                statements.push(Statement::ScoreboardObjectivesAdd(soa.clone()));
            }
            Token::ScoreboardPlayersSet(sps) => {
                statements.push(Statement::ScoreboardPlayersSet(sps.clone()));
            }
            Token::ScoreboardPlayersOperation(spo) => {
                statements.push(Statement::ScoreboardPlayersOperation(spo.clone()));
            }
            Token::FunctionCall(fc) => {
                statements.push(Statement::FunctionCall(fc.clone()));
            }
            _ => {}
        }

        i += 1;
    }

    func
}
