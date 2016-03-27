(function ($, window) {
    var directions = [[1, 0], [0, 1], [-1, 0], [0, -1]];
    var radius = 2;

    window.Game.Bomb = function (x, y, duration, power, bombType, player) {
        this.x = x;
        this.y = y;
        this.ticks = window.Game.TicksPerSecond * duration;
        this.order = 0;
        this.power = power;
        this.player = player;
        this.type = window.Game.Sprites.BOMB;
        this.bombType = bombType;
    };

    window.Game.Bomb.prototype = {
        update: function (game) {
            if (this.ticks > 0) {
                this.ticks--;
                if (this.ticks === 0) {
                    this.explode(game);
                }
            }
        },
        explode: function (game) {
            this.player.removeBomb();
            game.removeSprite(this);

            game.addSprite(new window.Game.Explosion(this.x, this.y, 0.5));
            for (var i = 0; i < directions.length; ++i) {
                var dir = directions[i];
                for (var j = 1; j <= radius; j++) {
                    var dx = dir[0] * j,
                      dy = dir[1] * j,
                      x = this.x + dx,
                      y = this.y + dy;
                    if (game.canDestroy(x, y)) {
                        game.addSprite(new window.Game.Explosion(x, y, 0.5));
                    } else {
                        break;
                    }
                }
            }
        }
    };

})(jQuery, window);