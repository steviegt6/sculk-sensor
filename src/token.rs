use logos::Logos;

#[derive(Logos, Debug, PartialEq, Clone)]
pub enum Token {
    #[token("_SCULK")]
    Sculk,

    #[token("_RET")]
    Ret,

    // Match _TMP0, _TMP1, _TMP123, etc.
    #[regex("_TMP[0-9]*", |lex| lex.slice()[4..].parse())]
    Tmp(i32),

    // Match _ARG<function_name>123, etc.
    #[regex(r"_ARG[A-za-z]+[0-9]+",  |lex| lex.slice()[4..].parse())]
    Arg(String),

    #[regex(r"[0-9]+", |lex| lex.slice().parse())]
    Number(i32),

    #[token("dummy")]
    Dummy,

    #[regex(r"#.+", |lex| lex.slice().parse())]
    Comment(String),

    #[token("scoreboard")]
    Scoreboard,

    #[token("objectives")]
    Objectives,

    #[token("add")]
    Add,

    #[token("players")]
    Players,

    #[token("operation")]
    Operation,

    #[token("+=")]
    PlusEqual,

    #[token("-=")]
    MinusEqual,

    #[token("*=")]
    TimesEqual,

    #[token("/=")]
    DivideEqual,

    #[token("%=")]
    ModuloEqual,

    #[token("=")]
    Equal,

    #[token("<")]
    LessThan,

    #[token(">")]
    GreaterThan,

    #[token("><")]
    Swap,

    #[token("set")]
    Set,

    #[regex(r"[ \t\r\n\f]+", logos::skip)]
    Whitespace,

    #[error]
    Error,
}

pub fn lexer(input: &str) -> impl Iterator<Item = Token> + '_ {
    Token::lexer(input)
}
