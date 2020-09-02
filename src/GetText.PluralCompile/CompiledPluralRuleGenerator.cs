﻿using System.Globalization;

using GetText.Plural;
using GetText.Plural.Ast;
using GetText.PluralCompile.Compiler;

namespace GetText.PluralCompile
{
    /// <summary>
    /// Plural rule generator that can parse a string that contains a plural rule and 
    /// compile it into a managed delegate.
    /// </summary>
    public class CompiledPluralRuleGenerator : AstPluralRuleGenerator
    {
        /// <summary>
        /// An instance of the <see cref="PluralRuleCompiler"/> class that will be used to 
        /// compile a plural rule abstract syntax tree to a managed delegate.
        /// </summary>
        public PluralRuleCompiler Compiler { get; protected set; }

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CompiledPluralRuleGenerator"/> class using 
        /// the default plural rule compiler and the default AST token parser.
        /// </summary>
        public CompiledPluralRuleGenerator()
            : this(new PluralRuleCompiler())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompiledPluralRuleGenerator"/> class using 
        /// given plural rule compiler and the default AST token parser.
        /// </summary>
        /// <param name="compiler"></param>
        public CompiledPluralRuleGenerator(PluralRuleCompiler compiler)
            : base()
        {
            Compiler = compiler;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompiledPluralRuleGenerator"/> class using 
        /// the default plural rule compiler and given AST token parser.
        /// </summary>
        /// <param name="parser"></param>
        public CompiledPluralRuleGenerator(AstTokenParser parser)
            : this(parser, new PluralRuleCompiler())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompiledPluralRuleGenerator"/> class using 
        /// given plural rule compiler and given AST token parser.
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="compiler"></param>
        public CompiledPluralRuleGenerator(AstTokenParser parser, PluralRuleCompiler compiler)
            : base(parser)
        {
            Compiler = compiler;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompiledPluralRuleGenerator"/> class using 
        /// the default plural rule compiler and the default AST token parser.
        /// </summary>
        /// <param name="pluralRuleText"></param>
        public CompiledPluralRuleGenerator(string pluralRuleText)
            : this(pluralRuleText, new PluralRuleCompiler())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompiledPluralRuleGenerator"/> class using 
        /// given plural rule compiler and the default AST token parser.
        /// </summary>
        /// <param name="pluralRuleText"></param>
        /// <param name="compiler"></param>
        public CompiledPluralRuleGenerator(string pluralRuleText, PluralRuleCompiler compiler)
            : base(pluralRuleText)
        {
            Compiler = compiler;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompiledPluralRuleGenerator"/> class using 
        /// the default plural rule compiler and given AST token parser.
        /// </summary>
        /// <param name="pluralRuleText"></param>
        /// <param name="parser"></param>
        public CompiledPluralRuleGenerator(string pluralRuleText, AstTokenParser parser)
            : this(pluralRuleText, parser, new PluralRuleCompiler())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompiledPluralRuleGenerator"/> class using 
        /// given plural rule compiler and given AST token parser.
        /// </summary>
        /// <param name="pluralRuleText"></param>
        /// <param name="parser"></param>
        /// <param name="compiler"></param>
        public CompiledPluralRuleGenerator(string pluralRuleText, AstTokenParser parser, PluralRuleCompiler compiler)
            : base(pluralRuleText, parser)
        {
            Compiler = compiler;
        }

        #endregion

        /// <summary>
        /// Creates a plural rule for given culture.
        /// </summary>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        public override IPluralRule CreateRule(CultureInfo cultureInfo)
        {
            if (PluralRuleText != null)
            {
                int numPlurals = ParseNumPlurals(PluralRuleText);
                string plural = ParsePluralFormulaText(PluralRuleText);
                Token astRoot = Parser.Parse(plural);

                PluralRuleEvaluatorDelegate evaulationDelegate = (PluralRuleEvaluatorDelegate)Compiler.CompileToDynamicMethod(astRoot, typeof(PluralRuleEvaluatorDelegate));

                return new CompiledPluralRule(numPlurals, evaulationDelegate);
            }

            return base.CreateRule(cultureInfo);
        }
    }
}
