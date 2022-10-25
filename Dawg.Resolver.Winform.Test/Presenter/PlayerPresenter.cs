using Dawg.Resolver.Winform.Test;

using DawgSolver.Repository;

namespace Dawg.Presenter
{
    public class PlayerPresenter
    {
        private readonly IPlayerView _view;
        private readonly IPlayerRepository _repository;

        public PlayerPresenter(IPlayerView view, IPlayerRepository repository )
        {
            _view = view;
            view.Presenter = this;
            _repository = repository;
        }
    }

   
}
