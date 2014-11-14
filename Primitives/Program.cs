using System;

namespace Primitives
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            //using (MainP game = new MainP())//Primitives Demo
            //using (MainB game = new MainB())//Buildings Demo
            //using (MainK game = new MainK()) //Streets Demo (Original)
            using (MainD game = new MainD()) //Buildings and Streets Demo
            //using (MainS game = new MainS()) //Projectiles Demo
            //using (MainT game = new MainT()) //Tank Demo
            {
                game.Run();
            }
        }
    }
#endif
}

