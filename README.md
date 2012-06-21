Apex Lumia
==========

Payload consisting of a Nokia Lumia 800 and NTX2 10mW transmitter.

A custom Windows Phone app, written in C#, gathers data from the various on-board sensors - the most important being the GPS. This is then constructed into a standard UKHAS-style sentence.

The sentence makes its way back to Earth in several ways. Firstly, when a data connection is available (typically below 8km), the phone will upload the sentences to Habitat's CouchDB itself. It will also tweet its location to an undecided twitter account.

However, this is certainly not its only method of communication. A radio transmitter is onboard. The phone has a headphone socket. This headphone socket can output a tone with varying amplitude, resulting in a varying voltage, which can then be fed through a simple AM demodulator resulting in beautiful RTTY entering the NTX2. This means that the payload can be tracked as normal via radio by trackers across the country for the full duration of its flight.

And of course, what is the point of a high altitude balloon launch without photos? The Lumia's camera will be running throughout, snapping pics and recording clips of video for everyone to gaze at when it returns to Earth.

It also does other stuff which you can work out by looking through the code ;)

## Useful links

* [Homepage](http://www.apexhab.org) - http://www.apexhab.org
* [Contact](mailto:team@apexhab.org) - team@apexhab.org
* [Twitter](http://twitter.com/apexhab) - http://twitter.com/apexhab

## Authors
* Daniel Saul
