#pragma once

#define SAMPLE_HOLD_LOW_MILLIS 18
#define DEFAULT_MAX_RETRIES 20

namespace Sensors
{
	namespace Dht
	{
		public value struct DhtReadingResult
		{
			bool TimedOut;
			bool IsValid;
			double Temperature;
			double Humidity;
			int RetryCount;
		};

		public interface class IDht
		{
			Windows::Foundation::IAsyncOperation<DhtReadingResult>^ GetReadingAsync();
			Windows::Foundation::IAsyncOperation<DhtReadingResult>^ GetReadingAsync(int maxRetries);
		};

		public ref class Dht11 sealed : IDht
		{
		public:
			Dht11(Windows::Devices::Gpio::GpioPin^ pin, Windows::Devices::Gpio::GpioPinDriveMode inputReadMode);
			virtual ~Dht11();

			virtual Windows::Foundation::IAsyncOperation<DhtReadingResult>^ GetReadingAsync();
			virtual Windows::Foundation::IAsyncOperation<DhtReadingResult>^ GetReadingAsync(int maxRetries);

		private:
			Windows::Devices::Gpio::GpioPinDriveMode _inputReadMode;
			Windows::Devices::Gpio::GpioPin^ _pin;

			DhtReadingResult InternalGetReading();
			DhtReadingResult Dht11::CalculateValues(std::bitset<40> bits);
		};
	}
}