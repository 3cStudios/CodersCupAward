namespace CodersCupAward.Exceptions
{

    
    public class FormDataException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMessage"></param>
        public FormDataException(string pMessage)
            : base(pMessage)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMessage"></param>
        /// <param name="pInnerException"></param>
        public FormDataException(string pMessage, Exception pInnerException)
            : base(pMessage, pInnerException)
        {
        }

        

        /// <summary>
        /// 
        /// </summary>
        public FormDataException()
        {
        }
    }
}
