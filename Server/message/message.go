package message

import (
	"encoding/json"
	"log"
	"net"
)

const (
	// MessageType is a broadcast type
	MessageType = iota
	// UserConnected is a broadcast type
	UserConnected = iota
	// UserDisconnected is a broadcast type
	UserDisconnected = iota
)

var (
	// Broadcast channel for new messages
	Broadcast = make(chan Message)
	// Clients is a map of connected clients
	Clients = make(map[net.Conn]User)
	// NextUUID for new clients
	NextUUID = 0
)

// Connection message sent immediately after connection is established
type Connection struct {
	Name  string `json:"name"`
	Email string `json:"email"`
}

// User has information about connected clients
type User struct {
	UUID  int
	Name  string
	Email string
}

// Message for recieving message from server
type Message struct {
	Type    int    `json:"type"`
	Message string `json:"message"`
	User    string `json:"user"`
}

// ConnectionMessage broadcasts when a user connects or disconnects
type ConnectionMessage struct {
	Type int    `json:"type"`
	User string `json:"user"`
}

// SuccessMessage returns a single bool stating success
type SuccessMessage struct {
	Success bool `json:"success"`
}

// HandleMessages by broadcasting them to all clients
func HandleMessages() {
	for {
		// Grab the next message from the broadcast channel
		msg := <-Broadcast
		switch msg.Type {
		case MessageType:
			msgBytes, err := json.Marshal(msg)
			if err != nil {
				log.Printf("Error encoding message: %v\n", err)
			}

			log.Printf("[Message] %v: %v\n", msg.User, msg.Message)
			broadcast(msgBytes)

		case UserConnected:
			connectionMessage := ConnectionMessage{
				Type: msg.Type,
				User: msg.User,
			}
			msgBytes, err := json.Marshal(connectionMessage)
			if err != nil {
				log.Printf("Error encoding message: %v\n", err)
			}

			log.Printf("%v has connected!", msg.User)
			broadcast(msgBytes)

		case UserDisconnected:
			connectionMessage := ConnectionMessage{
				Type: msg.Type,
				User: msg.User,
			}
			msgBytes, err := json.Marshal(connectionMessage)
			if err != nil {
				log.Printf("Error encoding message: %v\n", err)
			}

			log.Printf("%v has disconnected.", msg.User)
			broadcast(msgBytes)
		}
	}
}

func broadcast(msgBytes []byte) {
	// Send it out to every client that is currently connected
	for client := range Clients {
		_, err := client.Write(msgBytes)
		if err != nil {
			log.Printf("Sending message error: %v\n", err)
			client.Close()
			delete(Clients, client)
			Connected(UserDisconnected, Clients[client].Name)
		}
	}
}

// Connected broadcasts connection messages
func Connected(broadcastType int, name string) {
	connectionMessage := Message{
		Type: broadcastType,
		User: name,
	}
	Broadcast <- connectionMessage
}

// SendSuccessMessage sends a single bool stating success
func SendSuccessMessage(success bool, conn net.Conn) {
	successMessage := SuccessMessage{
		Success: success,
	}
	msgBytes, err := json.Marshal(successMessage)
	if err != nil {
		log.Printf("Error encoding message: %v\n", err)
	}
	conn.Write(msgBytes)
}
