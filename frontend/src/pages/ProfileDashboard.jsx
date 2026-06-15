// Importações / Dependências
import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { supabase } from '../services/supabase';


/* Fluxo de renderização - arquitetura cliente-servidor: */
/* 1. Verifica quem está logado. */
/* 2. Busca os dados físicos (peso, altura, etc.) na tabela perfis. */
/* 3. Dispara chamadas HTTP para os seus Controllers em C# (MetabolismoController, MacronutrientesController e IngestaoAguaController). */
/* 4. Exibe os resultados processados em blocos modernos e minimalistas. */


export function ProfileDashboard() {
  const navigate = useNavigate();
  const [carregando, setCarregando] = useState(true);
  const [erro, setErro] = useState('');

  // Estados para armazenar os dados de diferentes fontes
  const [perfil, setPerfil] = useState(null);
  const [resultadosMetabolismo, setResultadosMetabolismo] = useState(null);
  const [resultadosMacros, setResultadosMacros] = useState(null);
  const [resultadosAgua, setResultadosAgua] = useState(null);

  useEffect(() => {
    carregarDadosDoPainel();
  }, []);

  const carregarDadosDoPainel = async () => {
    try {
      // 1. Identificar o usuário
      const { data: { user } } = await supabase.auth.getUser();
      if (!user) {
        navigate('/');
        return;
      }

      // 2. Buscar o perfil no Supabase
      const { data: perfilData, error: perfilError } = await supabase
        .from('perfis')
        .select('*')
        .eq('id', user.id)
        .single();

      if (perfilError) throw perfilError;
      setPerfil(perfilData);

      // 3. Preparar o DTO de Request que o C# espera
      const requestPayload = {
        Nome: perfilData.nome,
        Idade: perfilData.idade,
        Sexo: perfilData.sexo,
        Peso: perfilData.peso,
        Altura: perfilData.altura,
        FatorAtividade: perfilData.fator_atividade,
        PercentualGordura: perfilData.percentual_gordura,
        ObjetivoFisico: perfilData.objetivo_fisico,
        FormulaUsada: 0 // Enum: 0 mapeia para MifflinStJeor no C#
      };

      // 4. Bater na sua API C# (Assumindo que ela roda na porta 5000 localmente)
      // Nota: O C# precisa estar rodando (dotnet run) para essas chamadas funcionarem!
      const apiUrl = 'http://localhost:5000/api';

      // Disparando as 3 requisições simultaneamente para máxima performance
      const [resMetabolismo, resMacros, resAgua] = await Promise.all([
        fetch(`${apiUrl}/metabolismo/calcular`, {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify(requestPayload)
        }),
        fetch(`${apiUrl}/macronutrientes/calcular`, {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify(requestPayload)
        }),
        fetch(`${apiUrl}/agua/calcular`, {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify(requestPayload)
        })
      ]);

      if (!resMetabolismo.ok || !resMacros.ok || !resAgua.ok) {
        throw new Error("Erro de comunicação com a API de cálculos do C#.");
      }

      // 5. Converter as respostas do C# para JSON
      setResultadosMetabolismo(await resMetabolismo.json());
      setResultadosMacros(await resMacros.json());
      setResultadosAgua(await resAgua.json());

    } catch (err) {
      setErro(err.message);
    } finally {
      setCarregando(false);
    }
  };

  const handleLogout = async () => {
    await supabase.auth.signOut();
    navigate('/');
  };

  // --- Design System: Minimalista e Profissional ---
  const containerStyle = {
    fontFamily: 'Arial, sans-serif', backgroundColor: '#f8fafc',
    minHeight: '100vh', padding: '40px 20px', color: '#334155'
  };

  const headerStyle = {
    maxWidth: '900px', margin: '0 auto 30px', display: 'flex',
    justifyContent: 'space-between', alignItems: 'center'
  };

  const gridStyle = {
    display: 'grid', gridTemplateColumns: 'repeat(auto-fit, minmax(280px, 1fr))',
    gap: '20px', maxWidth: '900px', margin: '0 auto'
  };

  const cardStyle = {
    backgroundColor: '#fff', borderRadius: '8px', padding: '20px',
    boxShadow: '0 4px 6px -1px rgba(0, 0, 0, 0.1)', border: '1px solid #e2e8f0'
  };

  const cardTitleStyle = {
    fontSize: '16px', fontWeight: 'bold', color: '#0f172a', margin: '0 0 15px 0',
    borderBottom: '2px solid #e2e8f0', paddingBottom: '8px'
  };

  const dataRowStyle = {
    display: 'flex', justifyContent: 'space-between', marginBottom: '10px', fontSize: '14.5px'
  };

  const badgeStyle = {
    backgroundColor: '#dbeafe', color: '#1e40af', padding: '4px 8px',
    borderRadius: '12px', fontSize: '12px', fontWeight: 'bold'
  };

  if (carregando) {
    return <div style={{ ...containerStyle, display: 'flex', alignItems: 'center', justifyContent: 'center' }}><h2>Sincronizando dados com o servidor...</h2></div>;
  }

  if (erro) {
    return (
      <div style={containerStyle}>
        <div style={{ maxWidth: '900px', margin: '0 auto', textAlign: 'center' }}>
          <h2 style={{ color: '#ef4444' }}>Ocorreu um problema</h2>
          <p>{erro}</p>
          <p style={{ fontSize: '13px', color: '#64748b' }}>Dica: Verifique se a sua API C# está rodando (dotnet run) na porta 5000.</p>
          <button onClick={() => window.location.reload()} style={{ padding: '10px 20px', marginTop: '20px', cursor: 'pointer' }}>Tentar Novamente</button>
        </div>
      </div>
    );
  }

  return (
    <div style={containerStyle}>
      <header style={headerStyle}>
        <div>
          <h1 style={{ margin: '0', fontSize: '24px', color: '#0f172a' }}>Olá, {perfil?.nome}</h1>
          <p style={{ margin: '5px 0 0 0', color: '#64748b' }}>@{perfil?.username} • Visão Geral do seu Metabolismo</p>
        </div>
        <div style={{ display: 'flex', gap: '10px' }}>
          {/* Novo Botão adicionado para ir ao Feed */}
          <button onClick={() => navigate('/feed')} style={{ padding: '8px 16px', backgroundColor: '#3b82f6', color: 'white', border: 'none', borderRadius: '6px', cursor: 'pointer', fontWeight: 'bold' }}>
            Ver Feed Social
          </button>
          <button onClick={handleLogout} style={{ padding: '8px 16px', backgroundColor: '#ef4444', color: 'white', border: 'none', borderRadius: '6px', cursor: 'pointer', fontWeight: 'bold' }}>
            Sair
          </button>
        </div>
      </header>

      <div style={gridStyle}>

        {/* Bloco 1: Estrutura Corporal */}
        <div style={cardStyle}>
          <h3 style={cardTitleStyle}>Estrutura Corporal</h3>
          <div style={dataRowStyle}><span>Peso Atual:</span> <strong>{perfil?.peso} kg</strong></div>
          <div style={dataRowStyle}><span>Altura:</span> <strong>{perfil?.altura} cm</strong></div>
          <div style={dataRowStyle}><span>IMC:</span> <strong>{resultadosMetabolismo?.imc}</strong></div>
          <div style={{ marginTop: '10px', textAlign: 'center' }}>
            <span style={badgeStyle}>{resultadosMetabolismo?.classificacaoIMC}</span>
          </div>
        </div>

        {/* Bloco 2: Motor Metabólico */}
        <div style={cardStyle}>
          <h3 style={cardTitleStyle}>Motor Metabólico (Kcal)</h3>
          <div style={dataRowStyle}><span>Taxa Basal (GER):</span> <strong>{resultadosMetabolismo?.ger} kcal</strong></div>
          <div style={dataRowStyle}><span>Gasto em Atividade (GAF):</span> <strong>{resultadosMetabolismo?.gaf} kcal</strong></div>
          <div style={dataRowStyle}><span>Efeito Térmico (ETA):</span> <strong>{resultadosMetabolismo?.eta} kcal</strong></div>
          <div style={{ ...dataRowStyle, marginTop: '15px', paddingTop: '10px', borderTop: '1px dashed #e2e8f0', color: '#059669', fontWeight: 'bold' }}>
            <span>Gasto Diário (GET):</span> <span>{resultadosMetabolismo?.get} kcal</span>
          </div>
        </div>

        {/* Bloco 3: Alvo Nutricional */}
        <div style={cardStyle}>
          <h3 style={cardTitleStyle}>Plano de {perfil?.objetivo_fisico}</h3>
          <div style={{ ...dataRowStyle, color: '#2563eb', fontWeight: 'bold' }}>
            <span>Meta Diária:</span> <span>{resultadosMacros?.caloriasAlvo} kcal</span>
          </div>
          <div style={dataRowStyle}><span>Proteínas:</span> <strong>{resultadosMacros?.proteina} g</strong></div>
          <div style={dataRowStyle}><span>Carboidratos:</span> <strong>{resultadosMacros?.carboidrato} g</strong></div>
          <div style={dataRowStyle}><span>Gorduras:</span> <strong>{resultadosMacros?.gordura} g</strong></div>
        </div>

        {/* Bloco 4: Hidratação */}
        <div style={cardStyle}>
          <h3 style={cardTitleStyle}>Hidratação Diária</h3>
          <div style={{ textAlign: 'center', margin: '20px 0' }}>
            <span style={{ fontSize: '32px', fontWeight: 'bold', color: '#0284c7' }}>{resultadosAgua?.totalLitros}</span>
            <span style={{ fontSize: '16px', color: '#64748b', marginLeft: '5px' }}>Litros</span>
          </div>
          <p style={{ textAlign: 'center', fontSize: '13px', color: '#64748b', margin: 0 }}>
            ({resultadosAgua?.totalMililitros} ml baseados em {resultadosAgua?.multiplicadorUsado}ml/kg)
          </p>
        </div>

      </div>
    </div>
  );
}
