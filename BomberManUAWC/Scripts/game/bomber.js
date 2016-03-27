(function ($, window) {
    var DELTA = 10,
        POWER = 100;

    window.Game.Bomber = function (handleInput) {
        this.x = 0;
        this.y = 0;
        this.exactX = 0;
        this.exactY = 0;
        this.canHandleInput = typeof handleInput === 'undefined' ? true : handleInput;

        // Debugging
        this.effectiveX = 0;
        this.effectiveY = 0;

        this.type = window.Game.Sprites.BOMBER;
        this.order = 2;
        this.maxBombs = 1;
        this.power = 1;
        this.speed = 1;
        this.directionX = 0;
        this.directionY = 0;
        this.ticks = window.Game.TicksPerSecond * 25;
        this.moving = true;
        this.activeFrameIndex = 0;

        this.direction = window.Game.Direction.SOUTH;
        this.bombs = 0;
        this.bombType = window.Game.Bombs.NORMAL;

        this.hasMoved = false;
        this.placedBomb = false;
    };

    window.Game.Bomber.prototype = {
        createBomb: function (game) {
            if (this.bombs >= this.maxBombs) {
                return;
            }
            this.placedBomb = true;
            this.bombs++;
            var bomb = new window.Game.Bomb(this.x, this.y, 3, this.power, this.bombType, this);
            game.addSprite(bomb);
        },
        handleInput: function (game) {
            if (game.inputManager.isKeyUp(window.Game.Keys.UP)) {
                this.directionY = 0;
            }

            if (game.inputManager.isKeyUp(window.Game.Keys.DOWN)) {
                this.directionY = 0;
            }

            if (game.inputManager.isKeyUp(window.Game.Keys.LEFT)) {
                this.directionX = 0;
            }

            if (game.inputManager.isKeyUp(window.Game.Keys.RIGHT)) {
                this.directionX = 0;
            }

            if (game.inputManager.isKeyDown(window.Game.Keys.UP)) {
                this.direction = window.Game.Direction.NORTH;
                this.directionY = -1;
            }

            if (game.inputManager.isKeyDown(window.Game.Keys.DOWN)) {
                this.direction = window.Game.Direction.SOUTH;
                this.directionY = 1;
            }

            if (game.inputManager.isKeyDown(window.Game.Keys.LEFT)) {
                this.direction = window.Game.Direction.WEST;
                this.directionX = -1;
            }

            if (game.inputManager.isKeyDown(window.Game.Keys.RIGHT)) {
                this.direction = window.Game.Direction.EAST;
                this.directionX = 1;
            }

            this.updateAnimation(game);

            if (game.inputManager.isKeyPress(window.Game.Keys.SPACE)) {
                this.createBomb(game);
            }
        },
        updateAnimation: function (game) {
            var moving = this.directionX !== 0 || this.directionY !== 0;

            if (moving) {
                if (!this.moving) {
                    this.moving = true;
                    this.frameLength = game.assetManager.getMetadata(this).frames[this.direction].length;
                    this.activeFrameIndex = 1;
                    this.movingTicks = 0;
                }
            }
            else {
                this.directionY = 0;
                this.directionX = 0;
                this.moving = false;
                this.activeFrameIndex = 0;
            }
        },
        update: function (game) {
            var x = this.exactX,
                y = this.exactY;

            if (this.canHandleInput) {
                this.handleInput(game);

                x += DELTA * this.directionX;
                y += DELTA * this.directionY;

                var oldX = this.x;
                var oldY = this.y;

                if (this.ticks > 0) {
                    this.ticks--;
                    if (this.ticks === 0) {
                        this.increaseMaxBombs();
                        this.ticks = window.Game.TicksPerSecond * 25;
                    }
                }
                this.moveExact(game, x, y);
                window.Game.Logger.log('max bombs = ' + this.maxBombs);
                window.Game.Logger.log('bombs = ' + this.bombs);
                if (this.x !== oldX || this.y !== oldY || this.placedBomb) {
                    this.hasMoved = true;
                    //console.log('player has moved');

                }
            }
            if (this.moving) {
                var frameRate = Math.floor(window.Game.TicksPerSecond / 2);
                this.movingTicks++;

                if (this.movingTicks % frameRate === 0) {
                    this.activeFrameIndex = (this.activeFrameIndex + 1) % this.frameLength;
                }
            }

            var sprites = game.getSpritesAt(this.x, this.y);
            for (var i = 0; i < sprites.length; ++i) {
                var sprite = sprites[i];
                if (sprite.type === window.Game.Sprites.POWERUP) {
                    switch (sprite.powerupType) {
                        case window.Game.Powerups.SPEED:
                            this.increaseSpeed();
                            break;
                        case window.Game.Powerups.BOMB:
                            this.increaseMaxBombs();
                            break;
                        case window.Game.Powerups.EXPLOSION:
                            this.increasePower();
                            break;
                    }
                    sprite.explode(game);
                }
            }
        },
        explode: function (game) {
            game.removeSprite(this);
        },
        removeBomb: function () {
            this.bombs--;
        },
        increaseSpeed: function () {
            this.speed++;
        },
        increaseMaxBombs: function () {
            this.maxBombs++;
        },
        increasePower: function () {
            this.power++;
        },
        getXHitTargets: function () {
            if (this.directionX === 1) {
                return [{ x: 1, y: -1 }, { x: 1, y: 0 }, { x: 1, y: 1 }];
            }
            else if (this.directionX === -1) {
                return [{ x: -1, y: -1 }, { x: -1, y: 0 }, { x: -1, y: 1 }];
            }
            return [];
        },
        getYHitTargets: function () {
            if (this.directionY === -1) {
                return [{ x: -1, y: -1 }, { x: 0, y: -1 }, { x: 1, y: -1 }];
            }
            else if (this.directionY === 1) {
                return [{ x: -1, y: 1 }, { x: 0, y: 1 }, { x: 1, y: 1 }];
            }

            return [];
        },
        getHitTargets: function () {
            var targets = [],
                xs = this.getXHitTargets();
            ys = this.getYHitTargets();

            for (var i = 0; i < xs.length; ++i) {
                targets.push(xs[i]);
            }

            for (var i = 0; i < ys.length; ++i) {
                targets.push(ys[i]);
            }

            return targets;
        },
        moveExact: function (game, x, y) {
            this.effectiveX = x / POWER;
            this.effectiveY = y / POWER;

            var actualX = Math.floor((x + (POWER / 2)) / POWER),
                actualY = Math.floor((y + (POWER / 2)) / POWER),
                targets = this.getHitTargets(),
                sourceLeft = this.effectiveX * game.map.tileSize,
                sourceTop = this.effectiveY * game.map.tileSize,
                sourceRect = {
                    left: sourceLeft,
                    top: sourceTop,
                    right: sourceLeft + game.map.tileSize,
                    bottom: sourceTop + game.map.tileSize
                },
                collisions = [],
                possible = [];



            window.Game.Logger.log('actualX=' + actualX + ', actualY=' + actualY);


            for (var i = 0; i < targets.length; ++i) {
                var tx = targets[i].x,
                    ty = targets[i].y,
                    targetX = actualX + tx,
                    targetY = actualY + ty,
                    left = (actualX + tx) * game.map.tileSize,
                    top = (actualY + ty) * game.map.tileSize,
                    targetRect = {
                        left: left,
                        top: top,
                        right: left + game.map.tileSize,
                        bottom: top + game.map.tileSize
                    },
                    movable = game.movable(Math.floor(left / game.map.tileSize),
                                           Math.floor(top / game.map.tileSize)),
                    intersects = window.Game.Utils.intersects(sourceRect, targetRect);

                if (!movable && intersects) {
                    collisions.push({ x: targetX, y: targetY });
                }
                else {
                    possible.push({ x: targetX, y: targetY });
                }
            }

            if (collisions.length === 0) {
                if (window.Game.MoveSprites) {
                    this.x = actualX;
                    this.y = actualY;

                    this.exactX = x;
                    this.exactY = y;
                }

                this.candidate = null;
            }
            else {
                var candidates = [],
                    candidate,
                    p1 = { x: actualX + this.directionX, y: actualY },
                    p2 = { x: actualX, y: actualY + this.directionY };

                for (var i = 0; i < possible.length; ++i) {
                    if (possible[i].x === p1.x && possible[i].y === p1.y) {
                        candidates.push({
                            directionX: this.directionX,
                            directionY: 0,
                            x: possible[i].x,
                            y: possible[i].y
                        });
                    }

                    if (possible[i].x === p2.x && possible[i].y === p2.y) {
                        candidates.push({
                            directionX: 0,
                            directionY: this.directionY,
                            x: possible[i].x,
                            y: possible[i].y
                        });
                    }
                }

                if (candidates.length == 1) {
                    candidate = candidates[0];
                }
                else if (candidates.length == 2) {
                    var minDistance;
                    for (var i = 0; i < candidates.length; ++i) {
                        var targetCandidate = candidates[i],
                            xs = (this.exactX - candidates[i].x * POWER),
                            ys = (this.exactY - candidates[i].y * POWER)
                        distance = xs * xs + ys * ys;

                        if (!minDistance || distance < minDistance) {
                            minDistance = distance;
                            candidate = targetCandidate;
                        }
                    }
                }

                if (candidate) {

                    var diffX = candidate.x * POWER - this.exactX,
                        diffY = candidate.y * POWER - this.exactY,
                        absX = Math.abs(diffX),
                        absY = Math.abs(diffY),
                        effectiveDirectionX = 0,
                        effectiveDirectionY = 0;

                    if (absX === 100) {
                        effectiveDirectionX = 0;
                    }
                    else {
                        effectiveDirectionX = window.Game.Utils.sign(diffX);
                    }

                    if (absY === 100) {
                        effectiveDirectionY = 0;
                    }
                    else {
                        effectiveDirectionY = window.Game.Utils.sign(diffY);
                    }

                    if (effectiveDirectionX === 0 && effectiveDirectionY === 0) {
                        effectiveDirectionX = candidate.directionX;
                        effectiveDirectionY = candidate.directionY;
                    }


                    if (window.Game.MoveSprites) {
                        this.setDirection(effectiveDirectionX, effectiveDirectionY);

                        this.exactX += DELTA * effectiveDirectionX;
                        this.x = actualX;

                        this.exactY += DELTA * effectiveDirectionY;
                        this.y = actualY;
                    }

                    this.candidate = candidate;
                }
                else {
                    var diffY = (collisions[0].y * POWER - this.exactY),
                        diffX = (collisions[0].x * POWER - this.exactX),
                        absX = Math.abs(diffX),
                        absY = Math.abs(diffY),
                        effectiveDirectionX = 0,
                        effectiveDirectionY = 0;

                    if (absX >= 35 && absX < 100) {
                        effectiveDirectionX = -window.Game.Utils.sign(diffX);
                    }

                    if (absY >= 35 && absY < 100) {
                        effectiveDirectionY = -window.Game.Utils.sign(diffY);
                    }

                    if (window.Game.MoveSprites) {
                        this.setDirection(effectiveDirectionX, effectiveDirectionY);

                        this.exactX += DELTA * effectiveDirectionX;
                        this.exactY += DELTA * effectiveDirectionY;
                    }

                    this.candidate = null;
                }
            }
        },
        setDirection: function (x, y) {
            if (x === -1) {
                this.direction = window.Game.Direction.WEST;
            }
            else if (x === 1) {
                this.direction = window.Game.Direction.EAST;
            }

            if (y === -1) {
                this.direction = window.Game.Direction.NORTH;
            }
            else if (y === 1) {
                this.direction = window.Game.Direction.SOUTH;
            }
        },
        moveTo: function (x, y) {
            this.exactX = x * POWER;
            this.exactY = y * POWER;
            this.effectiveX = x;
            this.effectiveY = y;
            this.x = x;
            this.y = y;
        }
    };

})(jQuery, window);