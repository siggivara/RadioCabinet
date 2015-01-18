#include  "msp430g2231.h"

 
 
void analogPinSelect(unsigned int pin){
  if(pin < 8){ 
    ADC10CTL0 &= ~ENC;
    ADC10CTL1 = pin << 12;
    ADC10CTL0 = ADC10ON + ENC + ADC10SHT_0;
  }
}

void adcInit(unsigned int pin) {
	DCOCTL = CALDCO_1MHZ;
  	BCSCTL1 = CALBC1_1MHZ;
  	analogPinSelect(pin);
}

unsigned int analogRead(){
	int i = 1;
	double val = 0;
	for (i=0; i<5; i++) {
		ADC10CTL0 |= ADC10SC;
  		while(ADC10CTL1 & ADC10BUSY);
  		val += ADC10MEM;
	}
  	return (val/i) + 1;
}

