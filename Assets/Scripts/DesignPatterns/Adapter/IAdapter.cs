using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DesignPatterns.Adapter
{
    public interface IAdapter<From,To>
    {
        To Convert(From reference);
    }
}
