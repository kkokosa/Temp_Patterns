using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using Spectre.Console;

namespace Liskov
{
    class Program
    {
        static void Main(string[] args)
        {
            var table = new Table()
                .Border(TableBorder.Rounded)
                .AddColumn(new TableColumn("[u]Account Type[/]"))
                .AddColumn("[u]Invariant Rule[/]")
                .AddColumn("[u]History Rule[/]")
                .AddColumn("[u]Exception Rule[/]")
                .AddColumn("[u]Precondition Rule[/]");

            foreach (bool useDerivedClass in new[] { false, true })
            {
                var results = new string[5];
                results[0] = useDerivedClass ? "SpecialAccount" : "BaseAccount";
                results[1] = ExecuteTest(useDerivedClass, TestInvariantRule, 100.0m);
                results[2] = ExecuteTest(useDerivedClass, TestHistoryRule, 100.0m);
                results[3] = ExecuteTest(useDerivedClass, TestExceptionRule, 12_000m);
                results[4] = ExecuteTest(useDerivedClass, TestPreconditionRule, 9_000m);

                table.AddRow(results);
            }

            AnsiConsole.Render(table);
        }

        static string ExecuteTest(bool useDerivedClass, Func<BaseAccount, decimal, string> testFunction, decimal amount)
        {
            BaseAccount account = !useDerivedClass
                ? new BaseAccount(50.0m)
                : new SpecialAccount(50.0m, 50.0m);

            return testFunction(account, amount);
        }


        private static string TestInvariantRule(BaseAccount account, decimal amount)
        {
            // Invariant is a non-negative balance
            var before = account.Balance >= 0;

            account.Withdraw(amount);

            var after = account.Balance >= 0;
            return (before == after ? "[green]OK[/]" : $"[red]Broken[/]\r\n{before}!={after}");
        }

        private static string TestHistoryRule(BaseAccount account, decimal amount)
        {
            decimal before = account.Balance;

            bool success = account.Withdraw(amount);

            decimal after = account.Balance;
            var invariant = success && before - after == amount ||
                            !success;
            return (invariant ? "[green]OK[/]" : $"[red]Broken[/]\r\n{before}-{after}!={amount}");
        }

        private static string TestExceptionRule(BaseAccount account, decimal amount)
        {
            try
            {
                var success = account.Withdraw(amount);
                return success.ToString();
            }
            catch (AccountException ex)
            {
                return "[green]OK[/]";
            }
            catch (Exception ex)
            {
                return $"[red]Broken[/]\r\n(by {ex.GetType()})";
            }
        }

        private static string TestPreconditionRule(BaseAccount account, decimal amount)
        {
            try
            {
                var success = account.Withdraw(amount);
                return "[green]OK[/]";
            }
            catch (AccountException ex)
            {
                return "[green]OK[/]";
            }
            catch (Exception ex)
            {
                return $"[red]Broken[/]\r\n(by {ex.GetType()})";
            }
        }
    }

    public class Result
    {
        public string Rule { get; set; }
        public string AccountType { get; set; }
        public string TestResult { get; set; }
    }

    internal class AccountException : Exception
    {
    }

    public class BaseAccount
    {
        protected decimal balance;

        public decimal Balance => balance;

        public BaseAccount(decimal balance)
        {
            this.balance = balance;
        }

        public virtual bool Withdraw(decimal amount)
        {
            if (amount >= 10_000m)
                throw new AccountException();

            if (amount <= Balance)
            {
                balance = Balance - amount;
                return true;
            }

            return false;
        }
    }

    public class SpecialAccount : BaseAccount
    {
        private const decimal SmallAccountDebit = 10.0m;
        protected decimal debit;

        public override bool Withdraw(decimal amount)
        {
            if (amount >= 8_000m)
                throw new InvalidOperationException();

            if (amount <= balance + SmallAccountDebit)
            {
                balance = balance - amount;
                return true;
            }

            if (amount <= balance + SmallAccountDebit + debit)
            {
                debit += amount - balance - SmallAccountDebit;
                balance = -SmallAccountDebit;
                return true;
            }

            return false;
        }

        public SpecialAccount(decimal balance, decimal debit) : base(balance)
        {
            this.debit = debit;
        }
    }
}