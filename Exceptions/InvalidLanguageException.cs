namespace CodersCupAward.Exceptions
{
    
    public class InvalidLanguageException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMessage"></param>
        public InvalidLanguageException(string pMessage)
            : base(pMessage)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMessage"></param>
        /// <param name="pInnerException"></param>
        public InvalidLanguageException(string pMessage, Exception pInnerException)
            : base(pMessage, pInnerException)
        {
        }

        
        /// <summary>
        /// 
        /// </summary>
        public InvalidLanguageException()
        {
        }
    }
}
