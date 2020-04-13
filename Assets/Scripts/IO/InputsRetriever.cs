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
            var inputs = new float[5];
            inputs[0] = (platform.transform.position.x) / 5;
            var ballPosition = platform.ball.transform.position;
            var ballVelocity = platform.ball.rb.velocity;
            inputs[1] = (ballPosition.x) / 5;
            inputs[2] = (ballPosition.y) / 5;
            inputs[3] = (ballVelocity.x) / 5;
            inputs[4] = (ballVelocity.y) / 5;

            return inputs;
        }
    }
}