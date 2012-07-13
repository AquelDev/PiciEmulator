using System;
using System.Collections.Generic;
using System.Text;
using Database_Manager.Database.Session_Details.Interfaces;

namespace Butterfly.Util
{
    class QueryChunk
    {
        private Dictionary<string, object> parameters;
        private StringBuilder queries;
        private int queryCount;
        private EndingType endingType;

        public QueryChunk()
        {
            parameters = new Dictionary<string, object>();
            queries = new StringBuilder();
            queryCount = 0;
            endingType = EndingType.Sequential;
        }

        public QueryChunk(string startQuery)
        {
            parameters = new Dictionary<string, object>();
            queries = new StringBuilder(startQuery);
            endingType = EndingType.Continuous;
            queryCount = 0;
        }

        internal void AddQuery(string query)
        {
            queryCount++;
            queries.Append(query);

            switch (endingType)
            {
                case EndingType.Continuous:
                    {
                        queries.Append(",");
                        break;
                    }
                case EndingType.Sequential:
                    {
                        queries.Append(";");
                        break;
                    }
            }
        }

        internal void AddParameter(string parameterName, object value)
        {
            parameters.Add(parameterName, value);   
        }

        internal void Execute(IQueryAdapter dbClient)
        {
            if (queryCount == 0)
                return;

            queries = queries.Remove(queries.Length - 1, 1);
            dbClient.setQuery(queries.ToString());

            foreach (KeyValuePair<string, object> parameter in parameters)
            {
                dbClient.addParameter(parameter.Key, parameter.Value);
            }

            dbClient.runQuery();
        }

        internal void Dispose()
        {
            parameters.Clear();
            queries.Clear();
            parameters = null;
            queries = null;
        }
    }

    enum EndingType
    {
        None,
        Sequential,
        Continuous
    }
}
