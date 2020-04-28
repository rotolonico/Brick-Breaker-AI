using System;
using System.Collections.Generic;
using System.Linq;
using AI;
using Game;
using UnityEngine;

namespace IO
{
    public static class InputsRetriever
    {
        public static float[] GetInputs(PlatformController platform)
        {
            var inputs = new float[6];
            var ballPosition = platform.ball.transform.position;
            var ballVelocity = platform.ball.rb.velocity;
            inputs[0] = (platform.transform.position.x - ballPosition.x) / 10 + 0.5f;
            inputs[1] = ballPosition.y / 5;
            inputs[2] = ballVelocity.x / 5;
            inputs[3] = ballVelocity.y / 5;
            var blocksRelativePosition = BlocksHandler.Instance.GetBlocksRelativePosition(ballPosition);
            inputs[4] = blocksRelativePosition[0];
            inputs[5] = blocksRelativePosition[1];

            return inputs;
        }
    }
}