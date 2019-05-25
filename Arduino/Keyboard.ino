#include "Keyboard.h"

void setup() {
  Keyboard.begin();
  Serial.begin(9600);
}
int key = 0;
void loop() {
  if (Serial.available() == 0)
    return;
  key = Serial.read();
  if (key < 100)
  {
    switch (key )
    {
      case 1: Keyboard.press(KEY_UP_ARROW); break;
      case 2: Keyboard.press(KEY_DOWN_ARROW); break;
      case 3: Keyboard.press(KEY_LEFT_ARROW); break;
      case 4: Keyboard.press(KEY_RIGHT_ARROW); break;
      case 5: Keyboard.press(32); break;
    }
  }
  else
  {
    key = key - 100;
    switch (key )
    {
      case 1: Keyboard.release(KEY_UP_ARROW); break;
      case 2: Keyboard.release(KEY_DOWN_ARROW); break;
      case 3: Keyboard.release(KEY_LEFT_ARROW); break;
      case 4: Keyboard.release(KEY_RIGHT_ARROW); break;
      case 5: Keyboard.release(32); break;
    }
  }
}

