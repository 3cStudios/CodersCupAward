namespace CodersCupAward.Exceptions
{

    public class MissingTemplateException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMessage"></param>
        public MissingTemplateException(string pMessage)
            : base(pMessage)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMessage"></param>
        /// <param name="pInnerException"></param>
        public MissingTemplateException(string pMessage, Exception pInnerException)
            : base(pMessage, pInnerException)
        {
        }

       
        /// <summary>
        /// 
        /// </summary>
        public MissingTemplateException()
        {
        }
    }
}
