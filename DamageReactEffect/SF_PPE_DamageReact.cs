using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EAttackDir
{
    none,
    left,
    right,
    up,
    down
}

[ExecuteInEditMode, RequireComponent(typeof(Camera))]
/// <summary>
/// screen effect, such as blood / burn / poison / hit and so on
/// </summary>
public class SF_PPE_DamageReact : MonoBehaviour
{

    #region VarDeclaration

    [SerializeField] Material m_material;        // for editor adjustment
    private Material m_replaceMat;
    [SerializeField] Texture2D leftTex, rightTex, upTex, downTex;
    [SerializeField, Header("[颜色]")] Color m_bloodColor;
    [SerializeField] Color m_poisonColor;
    private float fadeOutTime = 0.1f;       // also for interval between attacks
    private Color curOverlayColor;

    private bool m_bReadyForAttack = true;      // Not used due to using DOTween.Rewind
    private bool m_bReadyForBurn = true;
    private bool m_bReadyForPoison = true;
    private bool m_bReadyForSpray = true;
    private bool m_bInitialized = false;
    private bool m_bReadyForShake = true;

    // Material property name
    private string m_blendTexName = "_BlendTex";
    private string m_flowTexName = "_FlowTex";
    private string m_noiseTexName = "_Noise";
    private string m_burnTexName = "_BurnTex";

    private string m_directonName = "_Orient";
    private string m_weightName = "_Weight";
    private string m_liquidBlendName = "_FlowLiquidBlend";
    private string m_liquidColorName = "_LiquidColor";
    private string m_burnBlend = "_BurnBlend";
    private string m_verticalSweep = "_VerticalSweep";      // when using liquid, remember set this to 1
    private string m_overlayName = "_OverlayColor";         // RGB use Multiply & A use Add

    // Coroutine for burn / poison / spray
    private IEnumerator co_burnOff, co_poisonOff, co_sprayOff, co_burnToDie;

    // Tweener
    private Tweener tweenDirFadeIn, tweenBurn, tweenDeath, tweenSprayIn, tweenSprayOut, tweenScreenFade;

    #endregion

    // Use this for initialization
    void Awake()
    {
        InitParam();
        ToDefault();
    }

    /// <summary>
    /// Initialization
    /// </summary>
    public void InitParam()
    {
        if (m_bInitialized) return;

        if (m_material == null)
        {
            Debug.LogError("Confirm material have be assigned!", this);
        }
        else
        {
            m_replaceMat = Instantiate(m_material);
            if (m_replaceMat.shader != Shader.Find("Shader Forge/SF_PPE_EdgeOverlay"))
            {
                Debug.LogError("Wrong shader used in material", this);
            }
            else
            {
                // Initial Tween
                tweenDirFadeIn = m_replaceMat.DOFloat(1, m_weightName, 0.5f).SetAutoKill(false).Pause();
                tweenBurn = m_replaceMat.DOFloat(1f, m_burnBlend, 0.6f).SetAutoKill(false).Pause();
                tweenDeath = m_replaceMat.DOFloat(0.8f, m_liquidBlendName, 10f).SetAutoKill(false).Pause();
                tweenSprayIn = m_replaceMat.DOFloat(0.4f, m_liquidBlendName, 0.2f).SetAutoKill(false).Pause();
                tweenSprayOut = m_replaceMat.DOFloat(0f, m_verticalSweep, 1).SetAutoKill(false).Pause();
            }
            curOverlayColor = m_replaceMat.GetColor(m_overlayName);

        }

        m_bInitialized = true;
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (m_replaceMat)
        {
            Graphics.Blit(source, destination, m_replaceMat);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }


    #region 与外界通信的接口

    /// <summary>
    /// 被攻击，已做状态恢复，直接调用即可 
    /// </summary>
    /// <param name="direction"></param>
    public void EncounterAttack(EAttackDir direction)
    {
        //if (!m_bReadyForAttack) return;  

        switch (direction)
        {
            case EAttackDir.none:
                break;
            case EAttackDir.left:
                FadeIn(leftTex, 1);
                break;
            case EAttackDir.right:
                FadeIn(rightTex, 2);
                break;
            case EAttackDir.up:
                FadeIn(upTex, 3);
                break;
            case EAttackDir.down:
                FadeIn(downTex, 4);
                break;
            default:
                break;
        }

        //Debug.Log("DEBUG: Attacked");
    }

    /// <summary>
    ///被火烧，已做状态恢复，直接调用即可 
    /// </summary>
    public void Burn()
    {
        if (!m_bReadyForBurn) return;

        co_burnOff = BurnOff(1, 0.6f);
        if (null == tweenBurn)
        {
            Debug.LogError("Burn tweener missing");
        }
        else
        {
            //Debug.Log(tweenBurn.IsPlaying());

            if (!tweenBurn.IsPlaying())
            {
                tweenBurn.Rewind();
            }
            tweenBurn.Play().OnComplete(delegate
            {
                StartCoroutine(co_burnOff);
            });
        }

        //Debug.Log("燃烧");
    }

    /// <summary>
    /// 被放毒，已做状态恢复，直接调用即可
    /// </summary>
    public void Poisoning()
    {
        if (!m_bReadyForPoison) return;

        m_replaceMat.SetColor(m_liquidColorName, m_poisonColor);
        m_replaceMat.DOFloat(0.4f, m_liquidBlendName, 0.2f).OnComplete(delegate
        {
            co_poisonOff = PoisonOff(5, 0.5f);
            StartCoroutine(co_poisonOff);
        });

    }

    /// <summary>
    /// 自杀式爆破，已做状态恢复，直接调用即可
    /// </summary>
    public void SprayBlood()
    {
        // avoid to disturb <Die> call
        if (!m_bReadyForSpray) return;

        m_replaceMat.SetColor(m_liquidColorName, m_bloodColor);
        m_replaceMat.SetFloat(m_verticalSweep, 1);
        if (co_sprayOff != null)
        {
            StopCoroutine(co_sprayOff);
        }
        tweenSprayOut.Rewind();
        tweenSprayIn.Rewind();
        tweenSprayIn.Play().OnComplete(delegate
        {
            co_sprayOff = SprayOff(0.3f);
            StartCoroutine(co_sprayOff);
        });

        //Debug.Log("喷血");
    }

    /// <summary>
    /// 死亡，复活后需要调用ToDefault做状态恢复
    /// </summary>
    public void ToDie()
    {
        // Death and Spray use same material property
        tweenSprayIn.Rewind();
        tweenSprayOut.Rewind();
        if (co_sprayOff != null)
        {
            StopCoroutine(co_sprayOff);
        }

        m_replaceMat.SetFloat(m_verticalSweep, 1);
        m_replaceMat.SetColor(m_liquidColorName, m_bloodColor);

        if (!tweenDeath.IsPlaying())
        {
            tweenDeath.Rewind();
        }
        tweenDeath.Play();

        m_bReadyForSpray = false;
    }

    /// <summary>
    /// 燃烧后死亡
    /// </summary>
    public void BurnToDie()
    {
        co_burnToDie = BurnToDeath();
        StartCoroutine(co_burnToDie);
    }

    /// <summary>
    /// 屏幕淡出淡入
    /// </summary>
    /// <param name="bOut"></param>
    /// <param name="duration"></param>
    /// <param name="onFadeComplete"></param>
    public void ScreenFadeOutIn(bool bOut, float duration, System.Action onFadeComplete)
    {
        Color target = bOut ? Color.black : Color.white;
        target.a = 0;
        ScreenFadeStop();
        tweenScreenFade = m_replaceMat.DOColor(target, m_overlayName, duration).OnComplete(delegate
        {
            if (onFadeComplete != null)
            {
                onFadeComplete();
            }
        });
    }

    /// <summary>
    /// 屏幕淡出淡入
    /// </summary>
    public void ScreenFadeStop()
    {
        if (tweenScreenFade != null)
        {
            if (tweenScreenFade.IsComplete() == false)
            {
                tweenScreenFade.Complete();
            }
            tweenScreenFade.Kill();
            tweenScreenFade = null;
        }
    }

    /// <summary>
    /// 相机抖动
    /// </summary>
    /// <param name="strength"></param>
    /// <param name="shakeDuration"></param>
    public void ShakeCamera(float strength, float shakeDuration)
    {
        if (!m_bReadyForShake) return;

        bool hasParent = transform.parent ? true : false;
        // vr camera controlled by hard device
        GameObject shakeParent = new GameObject("ShakeCamParent");
        if (hasParent)
        {
            shakeParent.transform.SetParent(transform.parent);
        }
        transform.SetParent(shakeParent.transform);

        shakeParent.transform.DOShakeRotation(shakeDuration, strength).OnComplete(delegate
        {
            transform.parent = hasParent ? shakeParent.transform.parent : null;
            Destroy(shakeParent);
            m_bReadyForShake = true;
        });

        m_bReadyForShake = false;
    }

    /// <summary>
    /// 恢复至初始状态，一般是玩家死亡后
    /// </summary>
    public void ToDefault()
    {
        if (null != co_burnOff)
        {
            StopCoroutine(co_burnOff);
        }
        if (null != co_poisonOff)
        {
            StopCoroutine(co_poisonOff);
        }
        if (null != co_sprayOff)
        {
            StopCoroutine(co_sprayOff);
        }
        if (null != co_burnToDie)
        {
            StopCoroutine(co_burnToDie);
        }

        // Tweener
        if (tweenBurn != null && tweenDirFadeIn != null && tweenDeath != null)
        {
            tweenBurn.Rewind();
            tweenDirFadeIn.Rewind();
            tweenSprayIn.Rewind();
            tweenSprayOut.Rewind();
            tweenDeath.Rewind();
        }

        if (m_replaceMat)
        {
            m_replaceMat.SetFloat(m_liquidBlendName, 0);
            m_replaceMat.SetFloat(m_weightName, 0);
            m_replaceMat.SetFloat(m_burnBlend, 0);
            m_replaceMat.SetColor(m_overlayName, new Color(1, 1, 1, 0));
        }

        m_bReadyForBurn = true;
        m_bReadyForPoison = true;
        m_bReadyForSpray = true;
    }
    #endregion

    // dirNum : 1--Left  2--Right  3--Up  4--Down
    void FadeIn(Texture2D texture, float dirNum)
    {
        if (!m_replaceMat)
        {
            Debug.LogError("Material missing");
        }
        m_replaceMat.SetTexture(m_blendTexName, texture);
        m_replaceMat.SetFloat(m_directonName, dirNum);

        if (null == tweenDirFadeIn)
        {
            Debug.LogError("Tween missing");
        }
        else
        {
            tweenDirFadeIn.Rewind(false);
            tweenDirFadeIn.Play().OnComplete(delegate
            {
                //Debug.Log(tweenDirFadeIn.IsPlaying());  
                m_replaceMat.DOFloat(0, m_weightName, fadeOutTime);
            });
        }

    }

    /// <summary>
    /// Burn fade out
    /// </summary>
    /// <param name="waitTime"></param>
    /// <param name="fadeTime"></param>
    /// <returns></returns>
    IEnumerator BurnOff(float waitTime, float fadeTime)
    {
        m_bReadyForBurn = false;

        yield return new WaitForSeconds(waitTime);
        m_replaceMat.DOFloat(0f, m_burnBlend, fadeTime).OnComplete(delegate
        {
            m_bReadyForBurn = true;
        });
    }

    /// <summary>
    /// Poison fade out
    /// </summary>
    /// <param name="waitTime"></param>
    /// <param name="fadeTime"></param>
    /// <returns></returns>
    IEnumerator PoisonOff(float waitTime, float fadeTime)
    {
        m_bReadyForPoison = false;

        yield return new WaitForSeconds(waitTime);
        m_replaceMat.DOFloat(0f, m_liquidBlendName, fadeTime).OnComplete(delegate
        {
            m_bReadyForPoison = true;
        });
    }

    /// <summary>
    /// spray blood fade out
    /// </summary>
    /// <param name="waitTime"></param>
    /// <param name="fadeTime"></param>
    /// <returns></returns>
    IEnumerator SprayOff(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        tweenSprayOut.Rewind();
        tweenSprayOut.Play().OnComplete(delegate
        {
            m_replaceMat.SetFloat(m_liquidBlendName, 0);
            m_bReadyForSpray = true;
        });
    }

    IEnumerator BurnToDeath()
    {
        Burn();
        yield return new WaitForSeconds(0.4f);
        ToDie();
    }

    /// <summary>
    /// Debug material property value
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    float DebugFloatValue(string name)
    {
        return m_replaceMat.GetFloat(name);
    }

    #region 编辑器内测试使用
    public void OnTestAction(int dirNum)
    {
        switch (dirNum)
        {
            case 0:
                EncounterAttack(EAttackDir.left); break;
            case 1:
                EncounterAttack(EAttackDir.right); break;
            case 2:
                EncounterAttack(EAttackDir.up); break;
            case 3:
                EncounterAttack(EAttackDir.down); break;
            default:
                break;
        }
    }

    public void OnTestSetColor()
    {
        m_replaceMat.SetColor(m_liquidColorName, m_poisonColor);
        Debug.Log("DEBUG: Set Color");
    }

    public void OnTestShake()
    {
        ShakeCamera(5, 2);
    }
    #endregion
}
