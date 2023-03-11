use std::path::Path;

use crate::token::Token;

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

    Ok(context)
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
