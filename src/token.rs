use logos::{Lexer, Logos};

#[derive(Logos, Debug, PartialEq, Clone)]
pub enum Token {
    #[regex(r"[ \t\r\n\f]+", logos::skip)]
    Whitespace,

    #[regex(r"scoreboard objectives add (\S*) dummy", scoreboard_objectives_add)]
    ScoreboardObjectivesAdd(ScoreboardObjectivesAdd),

    #[regex(r"scoreboard players set (\S*) (\S*) [0-9]+", scoreboard_players_set)]
    ScoreboardPlayersSet(ScoreboardPlayersSet),

    #[regex(
        r"scoreboard players operation (\S*) (\S*) (\+=|-=|\*=|/=|%=|=) (\S*) (\S*)",
        scoreboard_players_operation
    )]
    ScoreboardPlayersOperation(ScoreboardPlayersOperation),

    #[regex(r"function (\S*)", function_call)]
    FunctionCall(FunctionCall),

    #[regex(r"#([^\r\n]*)", comment)]
    Comment(Comment),

    #[error]
    Error,
}

#[derive(Debug, PartialEq, Clone)]
pub enum Operation {
    Add,
    Subtract,
    Multiply,
    Divide,
    Modulo,
    Assign,
}

impl ToString for Operation {
    fn to_string(&self) -> String {
        match self {
            Operation::Add => "+=".to_string(),
            Operation::Subtract => "-=".to_string(),
            Operation::Multiply => "*=".to_string(),
            Operation::Divide => "/=".to_string(),
            Operation::Modulo => "%=".to_string(),
            Operation::Assign => "=".to_string(),
        }
    }
}

#[derive(Debug, PartialEq, Clone)]
pub struct ScoreboardObjectivesAdd {
    pub objective: String,
}

#[derive(Debug, PartialEq, Clone)]
pub struct ScoreboardPlayersSet {
    pub target: String,
    pub objective: String,
    pub score: i32,
}

#[derive(Debug, PartialEq, Clone)]
pub struct ScoreboardPlayersOperation {
    pub target: String,
    pub target_objective: String,
    pub operation: Operation,
    pub source: String,
    pub source_objective: String,
}

#[derive(Debug, PartialEq, Clone)]
pub struct FunctionCall {
    pub namespace: String,
    pub func_name: String,
}

#[derive(Debug, PartialEq, Clone)]
pub struct Comment {
    pub comment: String,
}

fn scoreboard_objectives_add(lex: &mut Lexer<Token>) -> Option<ScoreboardObjectivesAdd> {
    let mut iter = lex.slice().split_whitespace();
    iter.next();
    iter.next();
    iter.next();

    let objective = iter.next().unwrap().to_string();
    Some(ScoreboardObjectivesAdd { objective })
}

fn scoreboard_players_set(lex: &mut Lexer<Token>) -> Option<ScoreboardPlayersSet> {
    let mut iter = lex.slice().split_whitespace();
    iter.next();
    iter.next();
    iter.next();

    let target = iter.next().unwrap().to_string();
    let objective = iter.next().unwrap().to_string();
    let score = iter.next().unwrap().parse::<i32>().unwrap();
    Some(ScoreboardPlayersSet {
        target,
        objective,
        score,
    })
}

fn scoreboard_players_operation(lex: &mut Lexer<Token>) -> Option<ScoreboardPlayersOperation> {
    let mut iter = lex.slice().split_whitespace();
    iter.next();
    iter.next();
    iter.next();

    let target = iter.next().unwrap().to_string();
    let target_objective = iter.next().unwrap().to_string();
    let operation = match iter.next().unwrap() {
        "+=" => Operation::Add,
        "-=" => Operation::Subtract,
        "*=" => Operation::Multiply,
        "/=" => Operation::Divide,
        "%=" => Operation::Modulo,
        "=" => Operation::Assign,
        op => panic!("{}", format!("Invalid operation {}", op)),
    };
    let source = iter.next().unwrap().to_string();
    let source_objective = iter.next().unwrap().to_string();
    Some(ScoreboardPlayersOperation {
        target,
        target_objective,
        operation,
        source,
        source_objective,
    })
}

fn function_call(lex: &mut Lexer<Token>) -> Option<FunctionCall> {
    let mut iter = lex.slice().split_whitespace();
    iter.next();

    let call = iter.next().unwrap();
    let mut call_iter = call.split(':');
    let namespace = call_iter.next().unwrap().to_string();
    let func_name = call_iter.next().unwrap().to_string();
    Some(FunctionCall {
        namespace,
        func_name,
    })
}

fn comment(lex: &mut Lexer<Token>) -> Option<Comment> {
    let comment = lex.slice().to_string();
    Some(Comment { comment })
}

pub fn lexer(input: &str) -> impl Iterator<Item = Token> + '_ {
    Token::lexer(input)
}
