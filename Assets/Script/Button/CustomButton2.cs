using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace FGUIStarter
{
    public class CustomButton2 : Button, IPointerDownHandler, IPointerUpHandler
    {
        RectTransform textRect;
        Vector2 originalTextPos;
        TextMeshProUGUI textComponent;

        Image backgroundImage;

        [Header("Warna Teks Manual (Untuk Sprite Swap)")]
        public Color warnaTeksNormal = Color.white;
        public Color warnaTeksPressed = Color.gray;
        public Color warnaTeksDisabled = new Color(0.3f, 0.3f, 0.3f, 1f);

        [Header("Pengaturan Warna Latar Tombol")]
        public Color warnaTombolNormal = Color.white;
        public Color warnaTombolDisabled = new Color(0.4f, 0.4f, 0.4f, 1f);

        bool isHeld;
        protected override void Awake()
        {
            base.Awake();
            
            textComponent = GetComponentInChildren<TextMeshProUGUI>();
            if (textComponent != null)
            {
                textRect = textComponent.rectTransform;
                originalTextPos = textRect.anchoredPosition;
            }

            backgroundImage = GetComponent<Image>();
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            
            if (!interactable) return;

            isHeld = true;
            ApplyPressedVisual();
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);

            if (!interactable) return;

            isHeld = false;
            ApplyNormalVisual();
        }

        private void ApplyPressedVisual()
        {
            if (textRect != null)
            {
                float height = ((RectTransform)transform).rect.height;
                float offset = height - (height * 0.78718f);//calculation for 128x128 sprite
                //use this code below instead of the code in line 40, in case the offset of the text doesn't make sense with respect to the thickness of the button or gameview dimensions:
                //float offset = height - (height * 0.86718f) - insertYourCustomOffset;
                //Example: float offset = height - (height * 0.86718f) - 10f;
                textRect.anchoredPosition = originalTextPos - new Vector2(0, offset);
            }
        }

        private void ApplyNormalVisual()
        {
            if (textRect != null)
            {
                textRect.anchoredPosition = originalTextPos;
            }
        }

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            base.DoStateTransition(state, instant);

            if (state == SelectionState.Pressed)
            {
                ApplyPressedVisual();
            }
            else
            {
                ApplyNormalVisual();
            }

            if (textComponent != null)
            {
                switch (state)
                {
                    case SelectionState.Normal:
                    case SelectionState.Highlighted:
                    case SelectionState.Selected:
                        textComponent.color = warnaTeksNormal;
                        break;
                    case SelectionState.Pressed:
                        textComponent.color = warnaTeksPressed;
                        break;
                    case SelectionState.Disabled:
                        textComponent.color = warnaTeksDisabled;
                        break;
                }
            }

            if (backgroundImage != null)
            {
                if (state == SelectionState.Disabled)
                {
                    backgroundImage.color = warnaTombolDisabled; // Gelapkan saat terkunci
                }
                else
                {
                    backgroundImage.color = warnaTombolNormal; // Kembalikan normal (putih)
                }
            }
        }
    }
}
