// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/InputActions/Default.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace InputActionsClasses
{
    public class @Default : IInputActionCollection, IDisposable
    {
        public InputActionAsset asset { get; }
        public @Default()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""Default"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""1d90807b-59b3-4559-aa23-a0f2f956d740"",
            ""actions"": [
                {
                    ""name"": ""Button 1"",
                    ""type"": ""Button"",
                    ""id"": ""91020d50-5a26-451d-9eac-9b24c46ea602"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Button 2"",
                    ""type"": ""Button"",
                    ""id"": ""1c167f6e-01e6-4d9a-bc8b-9445ba10d890"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Button 3"",
                    ""type"": ""Button"",
                    ""id"": ""5239a5be-4d7e-4b87-b064-0ca50b25327d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Button 4"",
                    ""type"": ""Button"",
                    ""id"": ""4521c745-ad7c-4062-9ac2-e6ba4484ae9e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Up"",
                    ""type"": ""Button"",
                    ""id"": ""eefac5b4-15f9-486d-84ae-033adbabd211"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Down"",
                    ""type"": ""Button"",
                    ""id"": ""0b51bb0c-9cd9-4168-a41c-b5b786e0c57e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Left"",
                    ""type"": ""Button"",
                    ""id"": ""954d9872-a7c7-4c05-8424-805afdf798a8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Right"",
                    ""type"": ""Button"",
                    ""id"": ""5fea49bf-53c7-4596-a4b2-b6eb8b2f0277"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""4dca70af-cbc6-4f87-bca6-47bc8f3e070a"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Button 1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b46b15d8-8a0c-4af0-a622-f95181f7d352"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XInput"",
                    ""action"": ""Button 1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cf481b52-d0b8-45ab-9010-2e1fa58bf4c8"",
                    ""path"": ""<HID::. MAYFLASH Arcade Fightstick F500 V2>/trigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Button 1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6c3ed9fa-7302-4099-9cf2-4f115318032e"",
                    ""path"": ""<Keyboard>/t"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Button 2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0d437077-cb33-4d2a-92ec-3c4f764d6e1a"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XInput"",
                    ""action"": ""Button 2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""61d92017-718e-421b-adf7-7682c0c18668"",
                    ""path"": ""<HID::. MAYFLASH Arcade Fightstick F500 V2>/button2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Button 2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e03ab19e-fe79-403e-9780-a7c3a1b9dfa1"",
                    ""path"": ""<Keyboard>/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Button 3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""33f107c9-02e5-4f00-9232-2f1423666f6d"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XInput"",
                    ""action"": ""Button 3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8844d6b9-2ebd-4d4c-b12e-28013904e075"",
                    ""path"": ""<HID::. MAYFLASH Arcade Fightstick F500 V2>/button4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Button 3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d52c3c36-07cf-470b-9fa0-657a1e060837"",
                    ""path"": ""<Keyboard>/u"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Button 4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ee814f4e-043d-4b48-84b1-fcf80b6fc416"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XInput"",
                    ""action"": ""Button 4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8e413c65-58cc-4d09-90c8-415fba464d25"",
                    ""path"": ""<HID::. MAYFLASH Arcade Fightstick F500 V2>/button5"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Button 4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""be1a39e8-0939-4439-acfa-54df892780f0"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Up"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b944f7da-6a17-4425-abf8-76d9a6c4d589"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XInput"",
                    ""action"": ""Up"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a0775d46-564c-40ea-9514-a510033de2fc"",
                    ""path"": ""<HID::. MAYFLASH Arcade Fightstick F500 V2>/stick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Up"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e4163612-aa98-4bdc-97fe-27d6cf0b52b4"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Down"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2f0c1575-73b1-4954-8aa4-56be88ee4035"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XInput"",
                    ""action"": ""Down"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b6018a72-56c4-4dfc-83a1-ca46c3f01f40"",
                    ""path"": ""<HID::. MAYFLASH Arcade Fightstick F500 V2>/stick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Down"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6a7d7cae-8782-417f-9062-4fee4d43a1f4"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Left"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0bc5bb29-363e-4bef-ada8-834130b41a60"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XInput"",
                    ""action"": ""Left"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""96d9df3c-6e29-4402-a49c-e46059bc4f10"",
                    ""path"": ""<HID::. MAYFLASH Arcade Fightstick F500 V2>/stick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Left"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""35fe4933-4428-4e24-86a8-745014363169"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Right"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""34f6e611-f863-4eb2-bb97-28b9549b5cef"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XInput"",
                    ""action"": ""Right"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a7ba2ea7-00fd-4a0c-815c-d58beb43673e"",
                    ""path"": ""<HID::. MAYFLASH Arcade Fightstick F500 V2>/stick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Right"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""XInput"",
            ""bindingGroup"": ""XInput"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Hi"",
            ""bindingGroup"": ""Hi"",
            ""devices"": [
                {
                    ""devicePath"": ""<HID::. MAYFLASH Arcade Fightstick F500 V2>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
            // Player
            m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
            m_Player_Button1 = m_Player.FindAction("Button 1", throwIfNotFound: true);
            m_Player_Button2 = m_Player.FindAction("Button 2", throwIfNotFound: true);
            m_Player_Button3 = m_Player.FindAction("Button 3", throwIfNotFound: true);
            m_Player_Button4 = m_Player.FindAction("Button 4", throwIfNotFound: true);
            m_Player_Up = m_Player.FindAction("Up", throwIfNotFound: true);
            m_Player_Down = m_Player.FindAction("Down", throwIfNotFound: true);
            m_Player_Left = m_Player.FindAction("Left", throwIfNotFound: true);
            m_Player_Right = m_Player.FindAction("Right", throwIfNotFound: true);
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(asset);
        }

        public InputBinding? bindingMask
        {
            get => asset.bindingMask;
            set => asset.bindingMask = value;
        }

        public ReadOnlyArray<InputDevice>? devices
        {
            get => asset.devices;
            set => asset.devices = value;
        }

        public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

        public bool Contains(InputAction action)
        {
            return asset.Contains(action);
        }

        public IEnumerator<InputAction> GetEnumerator()
        {
            return asset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Enable()
        {
            asset.Enable();
        }

        public void Disable()
        {
            asset.Disable();
        }

        // Player
        private readonly InputActionMap m_Player;
        private IPlayerActions m_PlayerActionsCallbackInterface;
        private readonly InputAction m_Player_Button1;
        private readonly InputAction m_Player_Button2;
        private readonly InputAction m_Player_Button3;
        private readonly InputAction m_Player_Button4;
        private readonly InputAction m_Player_Up;
        private readonly InputAction m_Player_Down;
        private readonly InputAction m_Player_Left;
        private readonly InputAction m_Player_Right;
        public struct PlayerActions
        {
            private @Default m_Wrapper;
            public PlayerActions(@Default wrapper) { m_Wrapper = wrapper; }
            public InputAction @Button1 => m_Wrapper.m_Player_Button1;
            public InputAction @Button2 => m_Wrapper.m_Player_Button2;
            public InputAction @Button3 => m_Wrapper.m_Player_Button3;
            public InputAction @Button4 => m_Wrapper.m_Player_Button4;
            public InputAction @Up => m_Wrapper.m_Player_Up;
            public InputAction @Down => m_Wrapper.m_Player_Down;
            public InputAction @Left => m_Wrapper.m_Player_Left;
            public InputAction @Right => m_Wrapper.m_Player_Right;
            public InputActionMap Get() { return m_Wrapper.m_Player; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
            public void SetCallbacks(IPlayerActions instance)
            {
                if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
                {
                    @Button1.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnButton1;
                    @Button1.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnButton1;
                    @Button1.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnButton1;
                    @Button2.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnButton2;
                    @Button2.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnButton2;
                    @Button2.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnButton2;
                    @Button3.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnButton3;
                    @Button3.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnButton3;
                    @Button3.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnButton3;
                    @Button4.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnButton4;
                    @Button4.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnButton4;
                    @Button4.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnButton4;
                    @Up.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnUp;
                    @Up.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnUp;
                    @Up.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnUp;
                    @Down.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDown;
                    @Down.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDown;
                    @Down.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDown;
                    @Left.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLeft;
                    @Left.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLeft;
                    @Left.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLeft;
                    @Right.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRight;
                    @Right.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRight;
                    @Right.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRight;
                }
                m_Wrapper.m_PlayerActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Button1.started += instance.OnButton1;
                    @Button1.performed += instance.OnButton1;
                    @Button1.canceled += instance.OnButton1;
                    @Button2.started += instance.OnButton2;
                    @Button2.performed += instance.OnButton2;
                    @Button2.canceled += instance.OnButton2;
                    @Button3.started += instance.OnButton3;
                    @Button3.performed += instance.OnButton3;
                    @Button3.canceled += instance.OnButton3;
                    @Button4.started += instance.OnButton4;
                    @Button4.performed += instance.OnButton4;
                    @Button4.canceled += instance.OnButton4;
                    @Up.started += instance.OnUp;
                    @Up.performed += instance.OnUp;
                    @Up.canceled += instance.OnUp;
                    @Down.started += instance.OnDown;
                    @Down.performed += instance.OnDown;
                    @Down.canceled += instance.OnDown;
                    @Left.started += instance.OnLeft;
                    @Left.performed += instance.OnLeft;
                    @Left.canceled += instance.OnLeft;
                    @Right.started += instance.OnRight;
                    @Right.performed += instance.OnRight;
                    @Right.canceled += instance.OnRight;
                }
            }
        }
        public PlayerActions @Player => new PlayerActions(this);
        private int m_XInputSchemeIndex = -1;
        public InputControlScheme XInputScheme
        {
            get
            {
                if (m_XInputSchemeIndex == -1) m_XInputSchemeIndex = asset.FindControlSchemeIndex("XInput");
                return asset.controlSchemes[m_XInputSchemeIndex];
            }
        }
        private int m_KeyboardSchemeIndex = -1;
        public InputControlScheme KeyboardScheme
        {
            get
            {
                if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
                return asset.controlSchemes[m_KeyboardSchemeIndex];
            }
        }
        private int m_HiSchemeIndex = -1;
        public InputControlScheme HiScheme
        {
            get
            {
                if (m_HiSchemeIndex == -1) m_HiSchemeIndex = asset.FindControlSchemeIndex("Hi");
                return asset.controlSchemes[m_HiSchemeIndex];
            }
        }
        public interface IPlayerActions
        {
            void OnButton1(InputAction.CallbackContext context);
            void OnButton2(InputAction.CallbackContext context);
            void OnButton3(InputAction.CallbackContext context);
            void OnButton4(InputAction.CallbackContext context);
            void OnUp(InputAction.CallbackContext context);
            void OnDown(InputAction.CallbackContext context);
            void OnLeft(InputAction.CallbackContext context);
            void OnRight(InputAction.CallbackContext context);
        }
    }
}
