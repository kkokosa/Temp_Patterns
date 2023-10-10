using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWriting.Domain.Structure
{
    public enum TokenType
    {
        CHAPTER,
        FIGURE,
        PARAGRAPH,
        EOF
    }

    public class Token
    {
        public TokenType Type { get; }
        public string Value { get; }
        public int StartIndex { get; }
        public int EndIndex { get; }

        public Token(TokenType type, string value, int startIndex, int endIndex)
        {
            Type = type;
            Value = value;
            StartIndex = startIndex;
            EndIndex = endIndex;
        }
        public override string ToString()
            => Type != TokenType.PARAGRAPH
                ? $"Token({Type}, {Value}, {StartIndex}, {EndIndex})"
                : $"Token({Type}, {StartIndex}, {EndIndex})";
    }

    public class Lexer
    {
        private readonly string _input;
        private int _position = 0;
        private int _start = 0;

        public Lexer(string input)
        {
            _input = input;
        }

        public Token NextToken()
        {
            while (_position < _input.Length)
            {
                if (_input[_position] == '#')
                {
                    _start = _position;
                    while (_position < _input.Length && _input[_position] == '#') _position++;
                    if (_position < _input.Length && _input[_position] == ' ') _position++;
                    while (_position < _input.Length && _input[_position] != '\n') _position++;
                    return new Token(TokenType.CHAPTER, _input.Substring(_start, _position - _start).Trim(), _start,
                        _position);
                }

                if (_input[_position] == '!')
                {
                    _start = _position;
                    while (_position < _input.Length && _input[_position] != '\n') _position++;
                    return new Token(TokenType.FIGURE, _input.Substring(_start, _position - _start).Trim(), _start,
                        _position);
                }

                else if (!char.IsWhiteSpace(_input[_position]))
                {
                    _start = _position;
                    while (_position < _input.Length && _input[_position] != '\n') _position++;
                    return new Token(TokenType.PARAGRAPH, _input.Substring(_start, _position - _start).Trim(), _start,
                        _position);
                }

                _position++;
            }
            return new Token(TokenType.EOF, null, _input.Length, _input.Length);
        }
    }
}
