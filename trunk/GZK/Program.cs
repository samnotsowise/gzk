using System;

namespace GZK {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args) {
            using(GZK game = new GZK()) {
                game.Run();
            }
        }
    }
}

