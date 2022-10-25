using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dawg.Resolver.Winform.Test;

using DawgSolver.Repository;

namespace Dawg.Presenter
{
    public class MainPresenter
    {
        private readonly IMainView _view;
        private readonly IMainRepository _repository;

        public MainPresenter(IMainView view,IMainRepository repository )
        {
            _view = view;
            view.Presenter = this;
            _repository = repository;
        }
    }

   
}
