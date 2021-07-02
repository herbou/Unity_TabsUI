using UnityEngine ;
using UnityEngine.UI ;
using UnityEngine.Events ;

//------- Created by  : Hamza Herbou
//------- Email       : hamza95herbou@gmail.com

namespace EasyUI.Tabs {

   public class TabsUI : MonoBehaviour {

      [System.Serializable] public class TabsUIEvent : UnityEvent <int> {

      }

      [Header ("Tabs customization :")]
      [SerializeField] private Color themeColor = Color.gray ;
      [SerializeField] private float tabSpacing = 2f ;
      [Space]
      [Header ("OnTabChange event :")]
      public TabsUIEvent OnTabChange ;

      private TabButtonUI[] tabBtns ;
      private GameObject[] tabContent ;

      private HorizontalLayoutGroup tabsBtnsHorizontalLayoutGroup ;

      private Color tabColorActive, tabColorInactive ;
      private int current, previous ;
      private float tabHeightActive, tabHeightInactive ;

      private Transform parentBtns, parentContent ;

      private int tabBtnsNum, tabContentNum ;


      private void Start () {
         GetTabBtns () ;
      }

      private void GetTabBtns () {
         parentBtns = transform.GetChild (0) ;
         parentContent = transform.GetChild (1) ;
         tabBtnsNum = parentBtns.childCount ;
         tabContentNum = parentContent.childCount ;

         if (tabBtnsNum != tabContentNum) {
            Debug.LogError ("!!Number of <b>[Buttons]</b> is not the same as <b>[Contents]</b> ("
            + tabBtnsNum + " buttons & " + tabContentNum
            + " Contents)") ;
            return ;
         }

         tabBtns = new TabButtonUI[tabBtnsNum] ;
         tabContent = new GameObject[tabBtnsNum] ;
         for (int i = 0; i < tabBtnsNum; i++) {
            tabBtns [ i ] = parentBtns.GetChild (i).GetComponent <TabButtonUI> () ;
            int i_copy = i ;
            tabBtns [ i ].uiButton.onClick.RemoveAllListeners () ;
            tabBtns [ i ].uiButton.onClick.AddListener (() => OnTabButtonClicked (i_copy)) ;

            tabContent [ i ] = parentContent.GetChild (i).gameObject ;
            tabContent [ i ].SetActive (false) ;
         }

         previous = current = 0 ;

         tabColorActive = tabBtns [ 0 ].uiImage.color ;
         tabColorInactive = tabBtns [ 1 ].uiImage.color ;

         tabHeightActive = tabBtns [ 0 ].uiLayoutElement.preferredHeight ;
         tabHeightInactive = tabBtns [ 1 ].uiLayoutElement.preferredHeight ;

         tabBtns [ 0 ].uiButton.interactable = false ;
         tabContent [ 0 ].SetActive (true) ;
      }

      public void OnTabButtonClicked (int tabIndex) {
         if (current != tabIndex) {
            if (OnTabChange != null)
               OnTabChange.Invoke (tabIndex) ;

            previous = current ;
            current = tabIndex ;

            tabContent [ previous ].SetActive (false) ;
            tabContent [ current ].SetActive (true) ;

            tabBtns [ previous ].uiImage.color = tabColorInactive ;
            tabBtns [ current ].uiImage.color = tabColorActive ;

            tabBtns [ previous ].uiLayoutElement.preferredHeight = tabHeightInactive ;
            tabBtns [ current ].uiLayoutElement.preferredHeight = tabHeightActive ;

            tabBtns [ previous ].uiButton.interactable = true ;
            tabBtns [ current ].uiButton.interactable = false ;
         }
      }


      public void UpdateThemeColor (Color color) {
         tabBtns [ 0 ].uiImage.color = color ;
         Color colorDark = DarkenColor (color, 0.3f) ;
         for (int i = 1; i < tabBtnsNum; i++)
            tabBtns [ i ].uiImage.color = colorDark ;

         parentContent.GetComponent <Image> ().color = color ;
      }

      private Color DarkenColor (Color color, float amount) {
         float h, s, v ;
         Color.RGBToHSV (color, out h, out s, out v) ;
         v = Mathf.Max (0f, v - amount) ;
         return Color.HSVToRGB (h, s, v) ;
      }

      #if UNITY_EDITOR
      private void OnValidate () {
         parentBtns = transform.GetChild (0) ;
         parentContent = transform.GetChild (1) ;
         tabBtnsNum = parentBtns.childCount ;
         tabContentNum = parentContent.childCount ;

         tabBtns = new TabButtonUI[tabBtnsNum] ;
         tabContent = new GameObject[tabBtnsNum] ;

         for (int i = 0; i < tabBtnsNum; i++) {
            tabBtns [ i ] = parentBtns.GetChild (i).GetComponent <TabButtonUI> () ;
            tabContent [ i ] = parentContent.GetChild (i).gameObject ;
         }

         UpdateThemeColor (themeColor) ;

         if (tabsBtnsHorizontalLayoutGroup == null)
            tabsBtnsHorizontalLayoutGroup = parentBtns.GetComponent <HorizontalLayoutGroup> () ;
         tabsBtnsHorizontalLayoutGroup.spacing = tabSpacing ;

      }
      #endif

   }
}