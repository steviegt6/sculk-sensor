use std::path::Path;

use crate::token::{lexer, Token};

#[derive(Debug)]
pub struct DecompilerSettings {
    pub indent: usize,
}

#[derive(Debug)]
pub struct DecompilerContext {
    pub settings: DecompilerSettings,
    pub functions: Vec<Function>,
    pub entry_function: Option<Function>,
    pub main_function: Option<Function>,
}

#[derive(Debug)]
pub struct Function {
    pub name: String,
    pub body: Option<Vec<Statement>>,
}

#[derive(Debug)]
pub struct Statement {
    pub command: String,
    pub args: Vec<String>,
}

pub fn decompile(dir: &Path, settings: DecompilerSettings) -> Result<DecompilerContext, String> {
    let mut context = DecompilerContext {
        settings,
        functions: Vec::new(),
        entry_function: None,
        main_function: None,
    };

    match collect_functions(dir) {
        Ok((functions, main_function, entry_function)) => {
            context.functions = functions;
            context.main_function = main_function;
            context.entry_function = entry_function;
        }
        Err(e) => return Err(format!("error: {}", e)),
    }

    Ok(context)
}

fn collect_functions(
    dir: &Path,
) -> Result<(Vec<Function>, Option<Function>, Option<Function>), std::io::Error> {
    let mut functions = Vec::new();
    let mut entry_function = None;
    let mut main_function = None;

    for entry in std::fs::read_dir(dir)? {
        let entry = entry?;
        let path = entry.path();

        if path.is_dir() || path.extension().unwrap() != "mcfunction" {
            continue;
        }

        let file = std::fs::read_to_string(path)?;
        let _tokens = lexer(&file).collect::<Vec<_>>();

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
                entry_function = Some(Function {
                    name: String::from(name),
                    body: None,
                });
            }
            "main" => {
                main_function = Some(Function {
                    name: String::from(name),
                    body: None,
                });
            }
            _ => {
                functions.push(Function {
                    name: String::from(name),
                    body: None,
                });
            }
        }
    }

    Ok((functions, entry_function, main_function))
}

/*pub fn _decompile(input: &Vec<Token>, settings: DecompilerSettings) -> Result<String, String> {
    let mut output = String::new();
    let mut i = validate_is_sculk(input)?;

    // Since
    while i < input.len() {
        match input[i] {
            Token::Scoreboard => visit_scoreboard(&mut i, input, &mut output)?,
            _ => {
                i = input.len();
                return Err(format!(
                    "Invalid/unsupported token, broken control flow?: {:#?}",
                    input[i]
                ));
            }
        }
    }

    Ok(output.to_string())
}

fn validate_is_sculk(input: &Vec<Token>) -> Result<usize, String> {
    // Attempt to match "scoreboard objects add _SCULK dummy".
    if input[0] != Token::Scoreboard
        || input[1] != Token::Objectives
        || input[2] != Token::Add
        || input[3] != Token::Sculk
        || input[4] != Token::Dummy
    {
        return Err("Header check failed; not a sculk file".to_string());
    }

    Ok(5)
}

fn visit_scoreboard(i: &mut usize, input: &Vec<Token>, output: &mut String) -> Result<(), String> {
    let command = &input[*i + 1];
    match command {
        Token::Objectives => visit_players(i, input, output)?,
        Token::Players => visit_objectives(i, input, output)?,
        _ => {
            *i = input.len();
            return Err(format!(
                "Invalid/unsupported scoreboard command {:#?}",
                command
            ));
        }
    }

    Ok(())
}

fn visit_players(i: &mut usize, input: &Vec<Token>, output: &mut String) -> Result<(), String> {
    *i += 1;
    let command = &input[*i + 1];
    match command {
        Token::Operation => visit_players_operation(i, input, output)?,
        Token::Set
        _ => {
            *i = input.len();
            return Err(format!(
                "Invalid/unsupported scoreboard players command {:#?}",
                command
            ));
        }
    }

    Ok(())
}

fn visit_objectives(i: &mut usize, input: &Vec<Token>, output: &mut String) -> Result<(), String> {
    // *i += 1;
    *i = input.len();
    Err("scoreboard objectives not implemented".to_string())
}*/
