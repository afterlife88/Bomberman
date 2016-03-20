(function ($, window) {

    window.Game = {};

    window.Game.Sprites = {
        BOMB: 0,
        EXPLOSION: 1,
        BOMBER: 2,
        POWERUP: 3
    };

    window.Game.Keys = {
        Up: 38,
        Down: 40,
        Left: 37,
        Right:39,
        Space: 32
    };

    window.Game.Bombs = {
        NORMAL: 0,
        BOUNCY: 1,
        REMOTE: 2,
        P: 3
    };

    window.Game.Powerups = {
        SPEED: 0,
        BOMB: 1,
        EXPLOSION: 2
    };

    window.Game.Direction = {
        North: 0,
        South: 1,
        East: 2,
        West: 3
    };

    window.Game.TicksPerSecond = 60;
    window.Game.Debugging = false;
    window.Game.MoveSprites = false;


})(jQuery, window);