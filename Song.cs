using Godot;
using System;

public class Song : AudioStreamPlayer
{

	[Export] public int bpm = 128;
	[Export] public int measures = 4;

	//Tracking the beat and song position
	double _songPosition = 0.0;
	[Export] public int _songPositionInBeats = 1;
	private double _secPerBeat;
	private int _lastReportedBeat = 0;
	private int _beatsBeforeStart = 0;
	private int _measure = 1;

	//Determining how close to the beat an event is
	private int _closest = 0;
	private double _timeOffBeat = 0.0;

	[Signal] public delegate void Beat(double position);
	[Signal] public delegate void Measurer(double position);

	public override void _Ready()
	{
		_secPerBeat = 60.0 / bpm;
		Playing = true;
	}

	public override void _Process(float delta)
	{
		if (Playing == true)
		{
			_songPosition = GetPlaybackPosition() + AudioServer.GetTimeSinceLastMix();
			_songPosition -= AudioServer.GetOutputLatency();
			_songPositionInBeats = (int)(_songPosition / _secPerBeat) + _beatsBeforeStart;
			ReportBeat();
		}

	}

	public void ReportBeat()
	{
		if (_lastReportedBeat < _songPositionInBeats)
		{
				_measure = 1;
				EmitSignal("Beat", _songPositionInBeats);
				EmitSignal("Measurer", _measure);
				_lastReportedBeat = _songPositionInBeats;
				_measure += 1;
		}

	}

	public void PlayWithBeatOffset(int num)
	{
		_beatsBeforeStart = num;
		GetNode<Timer>("StartTimer").WaitTime = (float)_secPerBeat;
		GetNode<Timer>("StartTimer").Start();
	}

	public Vector2 ClosestBeat(int nth)
	{
		_closest = (int)(_songPosition / _secPerBeat / nth) * nth;
		_timeOffBeat = Math.Abs(_closest * _secPerBeat - _songPosition);
		return new Vector2(_closest, (float)_timeOffBeat);
	}

	private void PlayFromBeat(int beat, int offset)
	{
		Play();
		Seek(beat * (float)_secPerBeat);
		_beatsBeforeStart = offset;
		_measure = beat % measures;
	}

	public void _on_StartTimer_timeout()
	{
		_songPositionInBeats += 1;
		if (_songPositionInBeats < _beatsBeforeStart - 1)
			GetNode<Timer>("StartTimer").Start();
		else if (_songPositionInBeats == _beatsBeforeStart - 1)
		{
			GetNode<Timer>("StartTimer").WaitTime = (float)GetNode<Timer>("StartTimer").WaitTime - (float)(AudioServer.GetTimeToNextMix() + AudioServer.GetOutputLatency());
			GetNode<Timer>("StartTimer").Start();
		}
		else
		{
			Play();
			GetNode<Timer>("StartTimer").Stop();
			ReportBeat();
		}

	}
}
