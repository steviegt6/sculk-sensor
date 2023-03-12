mod decompiler;
mod token;

use crate::decompiler::{decompile, DecompilerSettings};
use std::{env::args, path::Path, process::exit};

fn main() {
    let args: Vec<String> = args().collect();
    assert!(!args.is_empty());

    if args.len() != 2 {
        println!("Usage: {} <directory>", args[0]);
        return;
    }

    let path = Path::new(&args[1]);
    if !path.is_dir() || !path.exists() {
        eprintln!("error: {} is not a directory", path.display());
        exit(1);
    }

    println!("path: {}", path.display());

    let settings = DecompilerSettings { indent: 4 };
    let context = decompile(path, settings);
    match context {
        Ok(ctx) => {
            println!("{:#?}", ctx);
            println!("{}", ctx.entry_function.text);
            for func in ctx.functions {
                println!("{}", func.text);
            }
        }
        Err(e) => {
            eprintln!("error: {}", e);
            exit(1);
        }
    }

    /*let file = std::fs::read_to_string(path).unwrap();
    let tokens = lexer(&file).collect::<Vec<_>>();

    println!("{:#?}", tokens);
    let output = decompile(&tokens);
    println!("{:#?}", output);*/
}
