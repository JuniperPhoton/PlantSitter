#include "MoistureSensor.h"
#include <stdio.h>
#include<wiringPi.h>

#define	Clock	27
#define	Address	28
#define	DataOut	29

unsigned int ADC_Read(unsigned char channel)
{
	unsigned int value;
	unsigned char i;
	unsigned char LSB = 0, MSB = 0;

	channel = channel << 4;
	for (i = 0; i < 4; i++)
	{
		if (channel & 0x80)
			digitalWrite(Address, 1);
		else
			digitalWrite(Address, 0);
		digitalWrite(Clock, 1);
		digitalWrite(Clock, 0);
		channel = channel << 1;
	}
	for (i = 0; i < 6; i++)
	{
		digitalWrite(Clock, 1);
		digitalWrite(Clock, 0);
	}

	delayMicroseconds(15);
	for (i = 0; i < 2; i++)
	{
		digitalWrite(Clock, 1);
		MSB <<= 1;
		if (digitalRead(DataOut))
			MSB |= 0x1;
		digitalWrite(Clock, 0);
	}
	for (i = 0; i < 8; i++)
	{
		digitalWrite(Clock, 1);
		LSB <<= 1;
		if (digitalRead(DataOut))
			LSB |= 0x1;
		digitalWrite(Clock, 0);
	}
	value = MSB;
	value <<= 8;
	value |= LSB;
	return value;
}


int main()
{
	if (setup() < 0)return 1;

	pinMode(DataOut, INPUT);
	pullUpDnControl(DataOut, PUD_UP);

	pinMode(Clock, OUTPUT);
	pinMode(Address, OUTPUT);


	while (1)
	{
		printf("AD: %d \n", ADC_Read(6));
		delay(1000);
	}
}

