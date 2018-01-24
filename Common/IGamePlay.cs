/************************************
 * 所有需要跟程序对接的效果控制，实现这个接口
 * 作者：饶牧旗(TA)   时间：2018年01月
 ***********************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGamePlay {

    //TODO : How to deal with specified location playing
    void Play(System.Action onExplosionComplete);

    void ToDefault();

    void Initialize();
}
