using WSLManager.IoC.Interfaces;
using WSLManager.Logger.Implementation;

namespace WSLManager.IoC.Base
{
    public class IoC
    {

        #region Public Properties
        
        public static BaseLogFactory baseFactory { get; private set; } = new BaseLogFactory();
        
        /// <summary>
        /// A shortcut to access the <see cref="ILogFactory"/>
        /// </summary>
        public static ILogFactory Logger => IoC.Get<ILogFactory>();
        
        #endregion
        
        #region Construction

        /// <summary>
        /// Sets up the IoC container, binds all information required and is ready for use
        /// NOTE: Must be called as soon as your application starts up to ensure all 
        ///       services can be found
        /// </summary>
        public static void Setup()
        {
            
        }

        #endregion
        
        /// <summary>
        /// Get's a service from the IoC, of the specified type
        /// </summary>
        /// <typeparam name="T">The type to get</typeparam>
        /// <returns></returns>
        // public static T Get<T>()
        // {
        //     return Logger.Get<T>();
        // }
        public static T Get<T>()
        {
            return baseFactory.Get<T>();
        }
    }
}