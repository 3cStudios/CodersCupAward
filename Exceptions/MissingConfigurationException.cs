namespace CodersCupAward.Exceptions
{

    public class MissingConfigurationException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMessage"></param>
        public MissingConfigurationException(string pMessage)
            : base(pMessage)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMessage"></param>
        /// <param name="pInnerException"></param>
        public MissingConfigurationException(string pMessage, Exception pInnerException)
            : base(pMessage, pInnerException)
        {
        }

     

        /// <summary>
        /// 
        /// </summary>
        public MissingConfigurationException()
        {
        }
    }
}
