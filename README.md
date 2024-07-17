# Silverfly

[![CodeFactor](https://www.codefactor.io/repository/github/furesoft/silverfly/badge)](https://www.codefactor.io/repository/github/furesoft/silverfly)
![NuGet Version](https://img.shields.io/nuget/v/silverfly)
![NuGet Downloads](https://img.shields.io/nuget/dt/Silverfly)
[![License: GPL v3](https://img.shields.io/badge/License-GPLv3-blue.svg)](https://www.gnu.org/licenses/gpl-3.0)
![Discord](https://img.shields.io/discord/455738571186241536)
![Libraries.io SourceRank](https://img.shields.io/librariesio/sourcerank/nuget/silverfly)

Silverfly is a versatile parsing framework that provides extensive support for building custom parsers with ease. It supports Pratt parsing, a powerful method for parsing expressions and statements in a flexible manner.

## Features

- **Flexible Parsing**: Supports Pratt parsing for complex expression handling.
- **Extensible**: Easily extend the parser and lexer with custom rules.
- **Documentation**: Comprehensive instructions available in the wiki.

## Installation

To install Silverfly, you can use NuGet:

```bash
dotnet add package Silverfly
```

## Usage

```csharp
﻿using Silverfly;

namespace Sample;

public class Program
{
    public static void Main(string[] args)
    {
        while (true)
        {
            Console.Write("> ");
            var input = Console.ReadLine();

            var parsed = Parser.Parse<ExpressionGrammar>(input);
            var evaluated = parsed.Tree.Accept(new EvaluationVisitor());

            Console.WriteLine("> " + evaluated);
        }
    }
}
```

For more detailed instructions and advanced usage, please refer to the [Wiki](https://github.com/furesoft/Silverfly/wiki).
A great example can be found [here](https://github.com/furesoft/Silverfly/tree/main/Source/Samples/Sample.FuncLanguage)

## Contributing
We welcome contributions! Please see our contributing guidelines for more details on how to get involved.
