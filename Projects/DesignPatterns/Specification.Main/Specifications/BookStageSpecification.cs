using System;
using System.Linq.Expressions;

namespace ConsoleApp
{
    public class BookStageSpecification : Specification<Book>
    {
        private readonly BookStage stage;

        public BookStageSpecification(BookStage stage)
        {
            this.stage = stage;
        }

        public override Expression<Func<Book, bool>> ToExpression()
        {
            return book => book.Stage == this.stage;
        }
    }
}
