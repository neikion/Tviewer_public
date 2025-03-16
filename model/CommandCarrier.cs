using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPF_Practice.model
{
    public abstract class CommandCarrierBase : ICommand
    {
        /* method를 여러개를 보관하는 구현도 있지만, 필요성을 느끼지 못하여 추가하지 않는다.
         * INotifyPropertyChanged를 통해 객체가 변경되면, 자동으로 CanExecuteChanged를 호출하여
         * 객체의 상태에 따라 실행조건은 확인하는 DelegateCommand는 참고할 가치가 있다.
         * 
         * CanExecuteChanged{
         *     add{ CommandManager.RequerySuggested += value;}
         *     remove{ CommandManager.RequerySuggested += value;}
         * }
         * 처럼 CommandManager.RequerySuggested를 사용하는 것은 성능상의 이슈가 있을 수 있다.
         * 위 코드는 커맨드의 변경사항을 확인하려고 하면
         * RequerySuggested(키보드, 마우스 관련 이벤트 발생 시 호출)의 등록된 모든 객체가 호출된다.
         * 
        */
        public event EventHandler? CanExecuteChanged;

        /// <summary>
        /// 실행에 영향을 미칠만한 변경점이 있을 경우 실행 조건을 다시 확인한다.<br/>
        /// UI쓰레드에서 호출해야된다.
        /// </summary>
        public void RaisesCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
        public abstract bool CanExecute(object? parameter);

        public abstract void Execute(object? parameter);
    }
    public class CommandCarrier : CommandCarrierBase
    {
        private Action<object?> _execute;
        private Predicate<object?>? _canExecute;
        public CommandCarrier(Action<object?> execute) : this(execute, null){}
        public CommandCarrier(Action<object?> execute, Predicate<object?>? canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
            _execute = execute;
            _canExecute = canExecute;
        }

        public override bool CanExecute(object? parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public override void Execute(object? parameter)
        {
            _execute(parameter);
        }
    }
    public class CommandCarrier<T> : CommandCarrierBase
    {
        public readonly Action<T?> _execute;
        public readonly Predicate<T?>? _canExecute;

        public CommandCarrier(Action<T?> execute) : this(execute, null){}

        public CommandCarrier(Action<T?> execute, Predicate<T?>? canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = canExecute;
        }

        public override bool CanExecute(object? parameter)
        {
            return _canExecute == null || _canExecute((T?)parameter);
        }

        public override void Execute(object? parameter)
        {
            _execute((T?)parameter);
        }
    }
}
