use crate::token::Token;

pub fn decompile(input: Vec<Token>) -> Result<String, String> {
    let mut output = String::new();

    validate_is_sculk(input)?;

    Ok(output.to_string())
}

fn validate_is_sculk(input: Vec<Token>) -> Result<(), String> {
    // Attempt to match "scoreboard objects add _SCULK dummy".
    if input[0] != Token::Scoreboard
        || input[1] != Token::Objectives
        || input[2] != Token::Add
        || input[3] != Token::Sculk
        || input[4] != Token::Dummy
    {
        return Err("Header check failed; not a sculk file".to_string());
    }

    Ok(())
}
