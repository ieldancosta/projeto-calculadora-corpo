import { useState } from 'react';

export function LoginPage() {
  const [email, setEmail] = useState('');
  const [senha, setSenha] = useState('');
  const [erro, setErro] = useState('');

  const handleLogin = (e) => {
    e.preventDefault();
    setErro('');

    if (!email || !senha) {
      setErro('Por favor, preencha todos os campos.');
      return;
    }

    alert(`Tentativa de login com:\nE-mail: ${email}\nSenha: ${senha}`);
  };

  // Estilos simples em linha para a página não ficar totalmente desalinhada
  const containerStyle = {
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'center',
    justifyContent: 'center',
    height: '100vh',
    fontFamily: 'Arial, sans-serif',
    backgroundColor: '#f4f4f9'
  };

  const formStyle = {
    backgroundColor: '#fff',
    padding: '30px',
    borderRadius: '8px',
    boxShadow: '0 4px 6px rgba(0,0,0,0.1)',
    width: '300px',
    display: 'flex',
    flexDirection: 'column',
    gap: '15px'
  };

  const inputStyle = {
    width: '100%',
    padding: '8px',
    borderRadius: '4px',
    border: '1px solid #ccc',
    boxSizing: 'border-box'
  };

  const buttonStyle = {
    width: '100%',
    padding: '10px',
    backgroundColor: '#007bff',
    color: 'white',
    border: 'none',
    borderRadius: '4px',
    cursor: 'pointer',
    fontWeight: 'bold'
  };

  return (
    <div style={containerStyle}>
      <form onSubmit={handleLogin} style={formStyle}>
        <h2 style={{ textAlign: 'center', margin: '0 0 10px 0', color: '#333' }}>Entrar no Sistema</h2>
        
        <div>
          <label style={{ display: 'block', marginBottom: '5px', fontSize: '14px' }}>E-mail</label>
          <input 
            type="email" 
            value={email}
            onChange={(e) => setEmail(e.target.value)} 
            placeholder="seu-email@exemplo.com"
            style={inputStyle}
          />
        </div>

        <div>
          <label style={{ display: 'block', marginBottom: '5px', fontSize: '14px' }}>Senha</label>
          <input 
            type="password" 
            value={senha}
            onChange={(e) => setSenha(e.target.value)} 
            placeholder="Digite sua senha"
            style={inputStyle}
          />
        </div>

        {erro && <p style={{ color: 'red', fontSize: '13px', margin: 0 }}>{erro}</p>}

        <button type="submit" style={buttonStyle}>Acessar</button>
      </form>
    </div>
  );
}