using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.G8.Exam.Infrastructure.DatabaseContext
{
	public class DatabaseContextFactory : IDatabaseContextFactory
	{
        private IDatabaseContext dataContext;

        /// <summary>
        /// Khởi tạo mới 1 Db Context nếu chưa có
        /// </summary>
        /// <returns>dataContext</returns>
        public IDatabaseContext Context()
        {
            return dataContext ?? (dataContext = new DatabaseContext());
        }

        /// <summary>
        /// Dispose existing context
        /// </summary>
        public void Dispose()
        {
            //if (dataContext != null)
            //    dataContext.Disposable();
        }
    }
}
