mod decompiler;
mod token;

use crate::decompiler::decompile;
use crate::token::lexer;
use std::{env::args, path::Path, process::exit};

fn main() {
    let args: Vec<String> = args().collect();
    assert!(!args.is_empty());

    if args.len() != 2 {
        println!("Usage: {} <file>", args[0]);
        return;
    }

    let path = Path::new(&args[1]);
    if !path.is_file() || !path.exists() {
        eprintln!("error: {} is not a file", path.display());
        exit(1);
    }

    // log path
    println!("path: {}", path.display());
    let file = std::fs::read_to_string(path).unwrap();
    let tokens = lexer(&file).collect::<Vec<_>>();

    println!("{:#?}", tokens);
    let output = decompile(&tokens);
    println!("{:#?}", output);
}
