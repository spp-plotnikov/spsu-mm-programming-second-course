using System;

namespace FibersProgram
{
    /// <summary>
    /// Потоки внутри потока, которые надо переключать вручную
    /// </summary>
    public class Fiber
    {

        /// <summary>
        /// The fiber action delegate.
        /// Событие файбера (делегат)
        /// </summary>
        private Action action;

        /// <summary>
        /// Gets the fiber identifier.
        /// Получаем идентификатор файбера
        /// </summary>
        public uint Id { get; private set; }

        /// <summary>
        /// Gets the id of the primary fiber.
        /// Получаем идентификатор начального(главного) файбера
        /// </summary>
        /// <remarks>If the Id is 0 then this means that there is no primary Id on the fiber.</remarks>
        /// <remarks>If если он 0, то это значит, что нет начального</remarks>
        public static uint PrimaryId { get; private set; }

        /// <summary>
        /// Gets the flag identifing the primary fiber (a fiber that can run other fibers).
        /// Получаем флаг обозначающий начальный файбер (файбер который может запускать другие файберы)
        /// </summary>
        public bool IsPrimary { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Fiber"/> class.
        /// Конструктор
        /// </summary>
        /// <param name='action'>Action transmitted to Fiber(?).</param>
        public Fiber(Action action)
        {
            InnerCreate(action);
        }

        /// <summary>
        /// Deletes the current fiber.
        /// </summary>
        /// <remarks>This method should only be used in the fiber action that's executing.</remarks>
        /// <remarks>Этот метод должен быть использован только для файбер-события исполняемого сейчас.</remarks>
        public void Delete()
        {
            UnmanagedFiberAPI.DeleteFiber(Id);
        }

        /// <summary>
        /// Deletes the fiber with the specified fiber id.
        /// Удалить файбер с указанным фибер-ид
        /// </summary>
        /// <param name='fiberId'>fiber id.</param>
        public static void Delete(uint fiberId)
        {
            UnmanagedFiberAPI.DeleteFiber(fiberId);
        }

        /// <summary>
        /// Switches the execution context to the next fiber.
        /// Переключение исполняемого контекста на следующий файбер
        /// </summary>
        /// <param name='fiberId'>Fiber id.</param>
        public static void Switch(uint fiberId)
        {
            Console.WriteLine(string.Format("Fiber [{0}] Switch", fiberId));
            UnmanagedFiberAPI.SwitchToFiber(fiberId);
        }

        /// <summary>
        /// Creates the fiber.
        /// </summary>
        /// <remarks>This method is responsible for the *actual* fiber creation.</remarks>
        /// <remarks>Этот метод отвечает за *фактическое* создание файбера.</remarks>
        /// <param name='action'>Fiber action.</param>
        private void InnerCreate(Action action)
        {
            this.action = action;

            if (PrimaryId == 0)
            {
                PrimaryId = UnmanagedFiberAPI.ConvertThreadToFiber(0);
                IsPrimary = true;
            }

            UnmanagedFiberAPI.LPFIBER_START_ROUTINE lpFiber = FiberRunnerProc;
            Id = UnmanagedFiberAPI.CreateFiber(100500, lpFiber, 0);
        }

        /// <summary>
        /// Fiber method that executes the fiber action.
        /// Метод файбера что запускает файбер-событие
        /// </summary>
        /// <param name='lpParam'>Lp parameter.</param>
        /// <returns>fiber status code.</returns>
        private uint FiberRunnerProc(uint lpParam)
        {
            uint status = 0;

            try
            {
                action();
            }
            catch (Exception)
            {
                status = 1;
                throw;
            }
            finally
            {
                if (status == 1)
                    UnmanagedFiberAPI.DeleteFiber((uint)Id);
            }

            return status;
        }
    }
}