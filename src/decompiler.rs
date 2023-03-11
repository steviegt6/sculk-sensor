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
    pub main_function: Option<Function>,
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

pub fn decompile(dir: &Path, settings: DecompilerSettings) -> Result<DecompilerContext, String> {
    let mut context = DecompilerContext {
        settings,
        name: None,
        functions: Vec::new(),
        entry_function: None,
        main_function: None,
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
                context.entry_function = Some(Function {
                    name: String::from(name),
                    body: Some({
                        let (name, body) = build_entry_body(tokens);
                        context.name = Some(name);
                        body
                    }),
                });
            }
            "main" => {
                context.main_function = Some(Function {
                    name: String::from(name),
                    body: Some(build_main_body(tokens)),
                });
            }
            _ => {
                context.functions.push(Function {
                    name: String::from(name),
                    body: Some(build_body(tokens)),
                });
            }
        }
    }

    Ok(context)
}

fn build_entry_body(tokens: Vec<Token>) -> (String, Vec<Statement>) {
    let statements = build_body(tokens);

    (String::new(), vec![])
}

fn build_main_body(tokens: Vec<Token>) -> Vec<Statement> {
    // print tokens for debug
    println!("{:#?}", tokens);

    vec![]
}

fn build_body(tokens: Vec<Token>) -> Vec<Statement> {
    let mut statements = Vec::new();
    let mut i = 0;

    /*while i < tokens.len() {
        match tokens[i] {
            Token::Scoreboard => {
                let (command, args) = build_scoreboard(&mut i, &tokens);
                statements.push(Statement { command, args });
            }
            Token::FunctionCall((namespace, func_name)) => statements.push(Statement {}),
            _ => {
                i = tokens.len();
                panic!(
                    "Invalid/unsupported token, broken control flow?: {:#?}",
                    tokens[i]
                );
            }
        }
    }*/
    println!("{:#?}", tokens);
    statements
}
