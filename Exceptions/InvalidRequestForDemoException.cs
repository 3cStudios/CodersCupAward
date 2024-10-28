namespace CodersCupAward.Exceptions
{
    public class InvalidRequestForDemoException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMessage"></param>
        public InvalidRequestForDemoException(string pMessage)
            : base(pMessage)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMessage"></param>
        /// <param name="pInnerException"></param>
        public InvalidRequestForDemoException(string pMessage, Exception pInnerException)
            : base(pMessage, pInnerException)
        {
        }

        

        /// <summary>
        /// 
        /// </summary>
        public InvalidRequestForDemoException()
        {
        }
    }
}
