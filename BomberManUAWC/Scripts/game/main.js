(function ($, window) {

    var requestAnimFrame = (function () {
        return function (callback) {
            window.setTimeout(callback, 1000 / window.Game.TicksPerSecond);
        };
    })();

    $(function () {
        var canvas = document.getElementById('canvas');
        var context = canvas.getContext('2d');
        // images etc
        var assetManager = new window.Game.AssetManager();
        // view engine
        var engine = new window.Game.Engine(assetManager);
        //window.engine = engine;
        var renderer = new window.Game.Renderer(assetManager);

        engine.initialize();
        animate(engine, renderer, canvas, context);
      
        $(document).keydown(function (e) {
            if (engine.onKeydown(e)) {
                e.preventDefault();
                return false;
            }
        });

        $(document).keyup(function (e) {
            if (engine.onKeyup(e)) {
                e.preventDefault();
                return false;
            }
        });
    });

    function animate(engine, renderer, canvas, context) {
        window.Game.Logger.clear();

        engine.update();
      
        context.clearRect(0, 0, canvas.width, canvas.height);
        if (engine.sprites.length === 1) {
            if (confirm('Game over! Want to play more? ')) {
                window.location.reload();
                return;
            }
        }
        renderer.draw(engine, context);

        // request new frame
        requestAnimFrame(function () {
            animate(engine, renderer, canvas, context);
        });
    }

})(jQuery, window);