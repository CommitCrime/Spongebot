using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SpongeBot.Bot
{
    class Bot
    {
        private log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        Timer botTimer;
        CancellationTokenSource botCancelTokenSource = new CancellationTokenSource();

        private readonly Input.Keyboard.ISendString keyboardInput;
        private string command;

        public Bot(Input.Keyboard.ISendString keyboardInput)
        {
            botTimer = new Timer(cast, null, -1, -1);
            this.keyboardInput = keyboardInput;
        }

        public void Start(String command)
        {
            this.command = command;
            this.botCancelTokenSource = new CancellationTokenSource();

            log.Info("Enable Bot");
            botTimer.Change(500, -1);
        }

        public void Stop()
        {
            this.botCancelTokenSource.Cancel();

            log.Info("Disable Bot");
            botTimer.Change(-1, -1);
        }

        private void cast(object state)
        {
            CancellationToken castTimeoutToken = new CancellationTokenSource(TimeSpan.FromSeconds(Properties.Settings.Default.CastDuration)).Token;
            CancellationTokenSource cancelTokens =
              CancellationTokenSource.CreateLinkedTokenSource(castTimeoutToken, botCancelTokenSource.Token);

            // 1.1 Cast Fishing (is quick → can be done synchronous )
            log.Debug("Send command.");
            keyboardInput.SendString(command);

            // 2.1 Listen for catch (can be done parallel)

            Task<bool> listener = Task.Run(function: async () => {
                Thread.Sleep(5 * 1000);
                if (cancelTokens.Token.IsCancellationRequested)
                {
                    log.Warn("Stop catch listener.");
                    cancelTokens.Token.ThrowIfCancellationRequested();
                }

                log.Info("Heard catch");
                return true;
            }, cancellationToken: cancelTokens.Token);

            // 2.1 Find Bobber
            log.Debug("Try to find bobber.");
            Task<System.Windows.Point> looker = Task.Run(function: async () => {
                Thread.Sleep(10 * 1000);
                if (cancelTokens.Token.IsCancellationRequested)
                {
                    log.Warn("Stop Bobber search");
                    cancelTokens.Token.ThrowIfCancellationRequested();
                }
                
                log.Info("Found Bobber");
                return new System.Windows.Point(5, 5);
            }, cancellationToken: cancelTokens.Token);

            // 3 Click Bobber
            Task.WhenAll(looker, listener).ContinueWith((a, token) =>
            {
                log.Debug("click on bobber");

            }, cancelTokens.Token, TaskContinuationOptions.OnlyOnRanToCompletion).ContinueWith((b) =>
            {
                // Repeat
                log.Debug("Repeat");
                cast(state);
            }, botCancelTokenSource.Token);
        }
    }
}
