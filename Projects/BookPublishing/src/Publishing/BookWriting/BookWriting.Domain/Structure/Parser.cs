using BookWriting.Domain.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BookWriting.Domain.Structure
{
    public class MarkdownParser
    {
        private readonly Lexer _lexer;
        private Token _currentToken;
        private ChapterElement _root;

        public MarkdownParser(Lexer lexer)
        {
            _lexer = lexer;
            _currentToken = _lexer.NextToken();
        }

        private void Eat(TokenType tokenType)
        {
            if (_currentToken.Type == tokenType)
                _currentToken = _lexer.NextToken();
            else
                throw new Exception($"Token type mismatch: expected {tokenType}, got {_currentToken.Type}");
        }

        public ChapterElement Parse()
        {
            Stack<ChapterElement> chapterStack = new Stack<ChapterElement>();

            while (_currentToken.Type != TokenType.EOF)
            {
                if (_currentToken.Type == TokenType.CHAPTER)
                {
                    var chapterLevel = _currentToken.Value.Count(ch => ch == '#');

                    while (chapterStack.Count >= chapterLevel)
                        chapterStack.Pop();

                    var chapter = new ChapterElement(_currentToken.StartIndex, _currentToken.EndIndex, _currentToken.Value.TrimStart('#').Trim());

                    if (chapterStack.Any())
                    {
                        chapterStack.Peek().Add(chapter);
                    }
                    else
                    {
                        _root = chapter; // Set the first chapter as the root
                    }

                    chapterStack.Push(chapter);
                    Eat(TokenType.CHAPTER);
                }
                else if (_currentToken.Type == TokenType.FIGURE)
                {
                    var match = Regex.Match(_currentToken.Value, @"!\[(.+)\]\((.+)\)");
                    if (match.Success)
                    {
                        var figure = new FigureElement(_currentToken.StartIndex, _currentToken.EndIndex, match.Groups[2].Value, match.Groups[1].Value);
                        chapterStack.Peek().Add(figure);
                    }
                    Eat(TokenType.FIGURE);
                }
                else if (_currentToken.Type == TokenType.PARAGRAPH)
                {
                    var paragraph = new ParagraphElement(_currentToken.StartIndex, _currentToken.EndIndex, _currentToken.Value);
                    chapterStack.Peek().Add(paragraph);
                    Eat(TokenType.PARAGRAPH);
                }
            }

            return _root;
        }
    }
}
