namespace CodersCupAward.Exceptions
{
    [Serializable]
    public class MissingRecordException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMessage"></param>
        public MissingRecordException(string pMessage)
            : base(pMessage)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMessage"></param>
        /// <param name="pInnerException"></param>
        public MissingRecordException(string pMessage, Exception pInnerException)
            : base(pMessage, pInnerException)
        {
        }

       
        /// <summary>
        /// 
        /// </summary>
        public MissingRecordException()
        {
        }
    }
}
