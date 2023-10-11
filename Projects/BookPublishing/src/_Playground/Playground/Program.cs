using BookWriting.Domain;
using BookWriting.Domain.Structure;

var input = @"# Markdown Example

## Introduction

This is an example of a Markdown file. Markdown is a lightweight markup language that you can use to add formatting elements to plaintext text documents.

## What is Markdown Used For?

Markdown is often used for:

- Writing documentation.
- Authoring blog posts.
- Making presentations.
- Writing scientific reports and papers.
- Creating eBooks.

## Features of Markdown

1. **Headers**: You can create headers using the `#` symbol followed by a space. The number of `#` symbols determines the header level.
2. **Emphasis**: You can emphasize text by wrapping it with `*` or `_`. For example, *italic* or _italic_, **bold** or __bold__.
3. **Lists**: You can create ordered and unordered lists using numbers or `-` and `*` symbols.

## Inserting Figures

You can also insert figures using the following format:

![Alt text for the image](URL_to_the_image)

For example:

![Example FigureElement](https://www.example.com/path/to/your/image.jpg)

> Note: The above URL is just a placeholder. Replace it with the actual link to your image.

## Conclusion

That's a brief overview of some of the features in Markdown. It's an incredibly powerful yet simple tool that's become indispensable for many people.
";
var lexer0 = new Lexer(input);
while (true)
{
    var token = lexer0.NextToken();
    Console.WriteLine(token);
    if (token.Type == TokenType.EOF)
    {
        break;
    }
}

var lexer = new Lexer(input);
var parser = new MarkdownParser(lexer);
var document = parser.Parse();
Console.WriteLine(document);


