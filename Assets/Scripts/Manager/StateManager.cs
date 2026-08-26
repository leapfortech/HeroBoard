using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Leap.Core.Tools;
using Leap.UI.Dialog;

using Sirenix.OdinInspector;
using Leap.Graphics.Tools;

public class StateManager : SingletonBehaviour<StateManager>
{
    private readonly String[] monthNames = { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };
    public String[] MonthNames => monthNames;

    // LoadBoard
    [Title("LoadBoard")]
    [SerializeField]
    int loadBoardTotal = 7;

    [SerializeField, Space]
    UnityEvent onBoardLoaded;

    private int loadCount = 0;
    public bool BoardLoading { get; set; } = false;

    public void BoardLoadZero() { loadCount = 0; BoardLoading = true; }

    public void BoardLoadHide()
    {
        if (!BoardLoading)
            ScreenDialog.Instance.Hide();
        else
            BoardLoadInc();
    }

    public bool BoardLoadInc()
    {
        if (!BoardLoading)
            return false;

        loadCount++;
        if (loadCount != loadBoardTotal)
            return true;

        onBoardLoaded.Invoke();

        loadCount = 0;
        BoardLoading = false;

        return true;
    }

    // BoardUser
    [Title("BoardUser")]
    [ShowInInspector, HideReferenceObjectPicker, ReadOnly]
    public BoardUser BoardUser { get; set; } = null;

    // Identity
    [Title("Identity")]
    [ShowInInspector, HideReferenceObjectPicker, ReadOnly]
    public Identity Identity { get; set; } = null;

    // Tale
    public List<TaleFull> TaleFulls { get; set; }
    private Dictionary<long, TaleFull> DictTaleFulls { get; set; } = new Dictionary<long, TaleFull>();
    private Dictionary<long, TaleFull> DictTaleFullsByPostId = new Dictionary<long, TaleFull>();

    public void ClearTale()
    {
        TaleFulls = null;
        DictTaleFulls.Clear();
        DictTaleFullsByPostId.Clear();
        taleImagesDic.Clear();
    }

    public TaleFull GetTaleFullById(long taleId)
    {
        if (!DictTaleFulls.TryGetValue(taleId, out TaleFull taleFull))
            return null;
        return taleFull;
    }

    public TaleFull GetTaleFullByPostId(long postId)
    {
        if (!DictTaleFullsByPostId.TryGetValue(postId, out TaleFull taleFull))
            return null;

        return taleFull;
    }

    public void AddTaleFull(TaleFull taleFull)
    {
        if (taleFull == null)
            return;

        if (TaleFulls == null)
            TaleFulls = new List<TaleFull>();

        DictTaleFulls[taleFull.Id] = taleFull;
        DictTaleFullsByPostId[taleFull.PostId] = taleFull;

        for (int i = 0; i < TaleFulls.Count; i++)
        {
            if (TaleFulls[i].Id == taleFull.Id)
            {
                TaleFulls[i] = taleFull;
                return;
            }
        }

        TaleFulls.Add(taleFull);
    }

    // Tale Images
    public List<Sprite> GetTaleImagesById(long taleId)
    {
        if (!taleImagesDic.TryGetValue(taleId, out List<Sprite> taleImages))
            return null;
        return taleImages;
    }

    private Dictionary<long, List<Sprite>> taleImagesDic = new Dictionary<long, List<Sprite>>();
    public void AddTaleImages(long taleId, List<Sprite> images)
    {
        taleImagesDic.Add(taleId, images);
    }

    // Treatment
    public List<TreatmentFull> TreatmentFulls { get; set; }
    private Dictionary<long, TreatmentFull> DictTreatmentFulls { get; set; } = new Dictionary<long, TreatmentFull>();
    private Dictionary<long, TreatmentFull> DictTreatmentFullsByPostId = new Dictionary<long, TreatmentFull>();

    public void ClearTreatment()
    {
        TreatmentFulls = null;
        DictTreatmentFulls.Clear();
        DictTreatmentFullsByPostId.Clear();
        treatmentImagesDic.Clear();
    }

    public TreatmentFull GetTreatmentFullById(long treatmentId)
    {
        if (!DictTreatmentFulls.TryGetValue(treatmentId, out TreatmentFull treatmentFull))
            return null;
        return treatmentFull;
    }

    public TreatmentFull GetTreatmentFullByPostId(long postId)
    {
        if (!DictTreatmentFullsByPostId.TryGetValue(postId, out TreatmentFull treatmentFull))
            return null;

        return treatmentFull;
    }

    public void AddTreatmentFull(TreatmentFull treatmentFull)
    {
        if (treatmentFull == null)
            return;

        if (TreatmentFulls == null)
            TreatmentFulls = new List<TreatmentFull>();

        DictTreatmentFulls[treatmentFull.Id] = treatmentFull;
        DictTreatmentFullsByPostId[treatmentFull.PostId] = treatmentFull;

        for (int i = 0; i < TreatmentFulls.Count; i++)
        {
            if (TreatmentFulls[i].Id == treatmentFull.Id)
            {
                TreatmentFulls[i] = treatmentFull;
                return;
            }
        }

        TreatmentFulls.Add(treatmentFull);
    }

    // Treatment Images
    public List<Sprite> GetTreatmentImagesById(long treatmentId)
    {
        if (!treatmentImagesDic.TryGetValue(treatmentId, out List<Sprite> treatmentImages))
            return null;
        return treatmentImages;
    }

    private Dictionary<long, List<Sprite>> treatmentImagesDic = new Dictionary<long, List<Sprite>>();
    public void AddTreatmentImages(long treatmentId, List<Sprite> images)
    {
        treatmentImagesDic.Add(treatmentId, images);
    }

    // Radio
    public List<RadioFull> RadioFulls { get; set; }
    private Dictionary<long, RadioFull> DictRadioFulls { get; set; } = new Dictionary<long, RadioFull>();
    private Dictionary<long, RadioFull> DictRadioFullsByPostId = new Dictionary<long, RadioFull>();

    public void ClearRadio()
    {
        RadioFulls = null;
        DictRadioFulls.Clear();
        DictRadioFullsByPostId.Clear();
        radioImagesDic.Clear();
    }

    public RadioFull GetRadioFullById(long radioId)
    {
        if (!DictRadioFulls.TryGetValue(radioId, out RadioFull radioFull))
            return null;
        return radioFull;
    }

    public RadioFull GetRadioFullByPostId(long postId)
    {
        if (!DictRadioFullsByPostId.TryGetValue(postId, out RadioFull radioFull))
            return null;

        return radioFull;
    }

    public void AddRadioFull(RadioFull radioFull)
    {
        if (radioFull == null)
            return;

        if (RadioFulls == null)
            RadioFulls = new List<RadioFull>();

        DictRadioFulls[radioFull.Id] = radioFull;
        DictRadioFullsByPostId[radioFull.PostId] = radioFull;

        for (int i = 0; i < RadioFulls.Count; i++)
        {
            if (RadioFulls[i].Id == radioFull.Id)
            {
                RadioFulls[i] = radioFull;
                return;
            }
        }

        RadioFulls.Add(radioFull);
    }

    // Radio Images
    public List<Sprite> GetRadioImagesById(long radioId)
    {
        if (!radioImagesDic.TryGetValue(radioId, out List<Sprite> radioImages))
            return null;
        return radioImages;
    }

    private Dictionary<long, List<Sprite>> radioImagesDic = new Dictionary<long, List<Sprite>>();
    public void AddRadioImages(long radioId, List<Sprite> images)
    {
        radioImagesDic.Add(radioId, images);
    }

    // Product
    public List<ProductFull> ProductFulls { get; set; }
    private Dictionary<long, ProductFull> DictProductFulls { get; set; } = new Dictionary<long, ProductFull>();
    private Dictionary<long, ProductFull> DictProductFullsByPostId = new Dictionary<long, ProductFull>();

    public void ClearProduct()
    {
        ProductFulls = null;
        DictProductFulls.Clear();
        DictProductFullsByPostId.Clear();
        productImagesDic.Clear();
    }

    public ProductFull GetProductFullById(long productId)
    {
        if (!DictProductFulls.TryGetValue(productId, out ProductFull productFull))
            return null;
        return productFull;
    }

    public ProductFull GetProductFullByPostId(long postId)
    {
        if (!DictProductFullsByPostId.TryGetValue(postId, out ProductFull productFull))
            return null;

        return productFull;
    }

    public void AddProductFull(ProductFull productFull)
    {
        if (productFull == null)
            return;

        if (ProductFulls == null)
            ProductFulls = new List<ProductFull>();

        DictProductFulls[productFull.Id] = productFull;
        DictProductFullsByPostId[productFull.PostId] = productFull;

        for (int i = 0; i < ProductFulls.Count; i++)
        {
            if (ProductFulls[i].Id == productFull.Id)
            {
                ProductFulls[i] = productFull;
                return;
            }
        }

        ProductFulls.Add(productFull);
    }

    // Product Images
    public List<Sprite> GetProductImagesById(long productId)
    {
        if (!productImagesDic.TryGetValue(productId, out List<Sprite> productImages))
            return null;
        return productImages;
    }

    private Dictionary<long, List<Sprite>> productImagesDic = new Dictionary<long, List<Sprite>>();
    public void AddProductImages(long productId, List<Sprite> images)
    {
        productImagesDic.Add(productId, images);
    }

    // Happening
    public List<HappeningFull> HappeningFulls { get; set; }
    private Dictionary<long, HappeningFull> DictHappeningFulls { get; set; } = new Dictionary<long, HappeningFull>();
    private Dictionary<long, HappeningFull> DictHappeningFullsByPostId = new Dictionary<long, HappeningFull>();

    public void ClearHappening()
    {
        HappeningFulls = null;
        DictHappeningFulls.Clear();
        DictHappeningFullsByPostId.Clear();
        happeningImagesDic.Clear();
    }

    public HappeningFull GetHappeningFullById(long happeningId)
    {
        if (!DictHappeningFulls.TryGetValue(happeningId, out HappeningFull happeningFull))
            return null;
        return happeningFull;
    }

    public HappeningFull GetHappeningFullByPostId(long postId)
    {
        if (!DictHappeningFullsByPostId.TryGetValue(postId, out HappeningFull happeningFull))
            return null;

        return happeningFull;
    }

    public void AddHappeningFull(HappeningFull happeningFull)
    {
        if (happeningFull == null)
            return;

        if (HappeningFulls == null)
            HappeningFulls = new List<HappeningFull>();

        DictHappeningFulls[happeningFull.Id] = happeningFull;
        DictHappeningFullsByPostId[happeningFull.PostId] = happeningFull;

        for (int i = 0; i < HappeningFulls.Count; i++)
        {
            if (HappeningFulls[i].Id == happeningFull.Id)
            {
                HappeningFulls[i] = happeningFull;
                return;
            }
        }

        HappeningFulls.Add(happeningFull);
    }

    // Happening Images
    public List<Sprite> GetHappeningImagesById(long happeningId)
    {
        if (!happeningImagesDic.TryGetValue(happeningId, out List<Sprite> happeningImages))
            return null;
        return happeningImages;
    }

    private Dictionary<long, List<Sprite>> happeningImagesDic = new Dictionary<long, List<Sprite>>();
    public void AddHappeningImages(long happeningId, List<Sprite> images)
    {
        happeningImagesDic.Add(happeningId, images);
    }

    // News
    public List<NewsFull> NewsFulls { get; set; }
    private Dictionary<long, NewsFull> DictNewsFulls { get; set; } = new Dictionary<long, NewsFull>();
    private Dictionary<long, NewsFull> DictNewsFullsByPostId = new Dictionary<long, NewsFull>();

    public void ClearNews()
    {
        NewsFulls = null;
        DictNewsFulls.Clear();
        DictNewsFullsByPostId.Clear();
        newsImagesDic.Clear();
    }

    public NewsFull GetNewsFullById(long newsId)
    {
        if (!DictNewsFulls.TryGetValue(newsId, out NewsFull newsFull))
            return null;
        return newsFull;
    }

    public NewsFull GetNewsFullByPostId(long postId)
    {
        if (!DictNewsFullsByPostId.TryGetValue(postId, out NewsFull newsFull))
            return null;

        return newsFull;
    }

    public void AddNewsFull(NewsFull newsFull)
    {
        if (newsFull == null)
            return;

        if (NewsFulls == null)
            NewsFulls = new List<NewsFull>();

        DictNewsFulls[newsFull.Id] = newsFull;
        DictNewsFullsByPostId[newsFull.PostId] = newsFull;

        for (int i = 0; i < NewsFulls.Count; i++)
        {
            if (NewsFulls[i].Id == newsFull.Id)
            {
                NewsFulls[i] = newsFull;
                return;
            }
        }

        NewsFulls.Add(newsFull);
    }

    // News Images
    public List<Sprite> GetNewsImagesById(long newsId)
    {
        if (!newsImagesDic.TryGetValue(newsId, out List<Sprite> newsImages))
            return null;
        return newsImages;
    }

    private Dictionary<long, List<Sprite>> newsImagesDic = new Dictionary<long, List<Sprite>>();
    public void AddNewsImages(long newsId, List<Sprite> images)
    {
        newsImagesDic.Add(newsId, images);
    }

    // Puzzle
    public List<PuzzleFull> PuzzleFulls { get; set; }
    private Dictionary<long, PuzzleFull> DictPuzzleFulls { get; set; } = new Dictionary<long, PuzzleFull>();
    private Dictionary<long, PuzzleFull> DictPuzzleFullsByPostId = new Dictionary<long, PuzzleFull>();

    public void ClearPuzzle()
    {
        PuzzleFulls = null;
        DictPuzzleFulls.Clear();
        DictPuzzleFullsByPostId.Clear();
    }

    public PuzzleFull GetPuzzleFullById(long puzzleId)
    {
        if (!DictPuzzleFulls.TryGetValue(puzzleId, out PuzzleFull puzzleFull))
            return null;
        return puzzleFull;
    }

    public PuzzleFull GetPuzzleFullByPostId(long postId)
    {
        if (!DictPuzzleFullsByPostId.TryGetValue(postId, out PuzzleFull puzzleFull))
            return null;

        return puzzleFull;
    }

    public void AddPuzzleFull(PuzzleFull puzzleFull)
    {
        if (puzzleFull == null)
            return;

        if (PuzzleFulls == null)
            PuzzleFulls = new List<PuzzleFull>();

        DictPuzzleFulls[puzzleFull.Id] = puzzleFull;
        DictPuzzleFullsByPostId[puzzleFull.PostId] = puzzleFull;

        for (int i = 0; i < PuzzleFulls.Count; i++)
        {
            if (PuzzleFulls[i].Id == puzzleFull.Id)
            {
                PuzzleFulls[i] = puzzleFull;
                return;
            }
        }

        PuzzleFulls.Add(puzzleFull);
    }



    public void ClearAll()
    {
        BoardUser = null;
        Identity = null;

        ClearTale();
        ClearTreatment();
        ClearRadio();
        ClearProduct();
        ClearHappening();
        ClearNews();
        ClearPuzzle();
    }

   

    // IdentityFull
    //[Title("IdentityFull")]
    //[ShowInInspector, HideReferenceObjectPicker, ReadOnly]
    //public List<IdentityFull> IdentityFulls
    //{
    //    get => identityFulls;
    //    set
    //    {
    //        identityFulls = value;
    //        identityFullDict = new Dictionary<long, IdentityFull>(identityFulls.Count);
    //        for (int i = 0; i < identityFulls.Count; i++)
    //            identityFullDict.Add(identityFulls[i].Id, identityFulls[i]); //RM BEFORE AppUserId
    //    }
    //}
    //Dictionary<long, IdentityFull> identityFullDict = new Dictionary<long, IdentityFull>();
    //List<IdentityFull> identityFulls = new List<IdentityFull>();
    //public IdentityFull GetIdentityFull(int appUserId) => identityFullDict.TryGetValue(appUserId, out IdentityFull identityFull) ? identityFull : null;

    //// Onboarding
    //public AppUserNamed[] AppUsers { get; set; } = null;
    //public int AppUserIdx { get; set; } = -1;
    //public AppUserNamed AppUser => AppUserIdx == -1 ? null : AppUsers[AppUserIdx];


    //public Identity AppUserIdentity { get; set; } = null;
    //public Address AppUserAddress { get; set; } = null;

    //public void ClearOnboardings()
    //{
    //    AppUserIdentity = null;
    //    AppUserAddress = null;
    //}
}