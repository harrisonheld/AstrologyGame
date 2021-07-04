using System;
using System.Collections.Generic;
using System.Text;

namespace AstrologyGame.Systems
{
    class SystemBase
    {
        public readonly Type[] Includes; // operates on entities that has all these component types
        public readonly Type[] Excludes; // will not operate on entities if they have any of these types
    }
}
